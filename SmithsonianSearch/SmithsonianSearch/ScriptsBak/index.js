window.Smithsonian = {
    Search: {
        isConsoleDefined : (typeof console != "undefined"),

        selectors : {
            loadingBlock: '#loading_text',
            resultContainer: '#result',
            searchForm: 'form#search',
            queryId: 'searchPhrase',
            queryInput: 'input#searchPhrase',
            clearSearchBoxLink: "#search-box #clearSearchBox",
            lastSearchUrl: 'form#backButtonHelperForm input[name=lastSearchUrl]',
            resultItemsContainer: 'ul.search-results, table.searchResultsTable tbody',
            endOfResultsContainer: '#search-results:parent',
            nextLink: 'a.nextLink',
            visibleNextLink: 'a.nextLink:visible',
            visibleNextLinkRow: 'table tr.nextLinkRow:visible',
            lazyLoadingMainContainer: 'body',
            addTocartPopupMessage: '#addToCartMessage',
            backToTopLink: "a.back-to-top",
            addThisToolbox: '.addthis_toolbox',
            buyForm: 'form.buy',
            dropDownLists: '#sortOption select, #tabsMobile select',
            filterCheckboxes: 'input[type=checkbox].filter'
        },

        LazyLoading: {

            loading: false,

            endOfResults: false,

            reset: function () {
                if (Smithsonian.Search.isConsoleDefined) { console.log('lazy loading reset.'); }

                $('#endOfResults').remove();
                Smithsonian.Search.LazyLoading.endOfResults = false;
                $("#search-results").removeClass("endOfResults");
            },

            setEndOfResults: function () {
                Smithsonian.Search.LazyLoading.loading = false;
                Smithsonian.Search.LazyLoading.endOfResults = true;
                $("#search-results").addClass("endOfResults");
            },

            loadPage: function (url) {
                if (Smithsonian.Search.isConsoleDefined) { console.log('loadPage'); }

                if (typeof url == "undefined" || url == '') {
                    if (Smithsonian.Search.isConsoleDefined) { console.error("loadPage: URL for Ajax request is undefined or empty.", this); }
                    return;
                }

                var container = $(Smithsonian.Search.selectors.lazyLoadingMainContainer);
                $(container).addClass('loading');
                $.ajax({
                    url: url,
                    method: "get",
                    cache: true,
                    success: function (result) {
                        if (Smithsonian.Search.isConsoleDefined) { console.log("load page: success"); }

                        // Remove previous "see more" link and its container row in case of table
                        $(Smithsonian.Search.selectors.nextLink).remove();
                        $(Smithsonian.Search.selectors.visibleNextLinkRow).remove();

                        // Insert result HTML into page
                        $(Smithsonian.Search.selectors.resultItemsContainer).append(result);

                        $(container).removeClass('loading');
                        Smithsonian.Search.LazyLoading.loading = false;
                    },

                    error: function (error) {
                        if (Smithsonian.Search.isConsoleDefined) {
                            console.error("loadPage: error downloading a page.", JSON.stringify(error));
                        }

                        $(container).removeClass('loading');
                        Smithsonian.Search.LazyLoading.loading = false;
                    }
                });

            },

            setUpLazyLoading: function (firstTimeLoad) {

                Smithsonian.Search.LazyLoading.reset();

                if (firstTimeLoad) {

                    $(window).scroll(function () {

                        if (Smithsonian.Search.LazyLoading.loading != true &&
                            ($(window).scrollTop() > $(document).height() - $(window).height() - 100) &&
                            Smithsonian.Search.isAnyContentInPlace()) {

                            Smithsonian.Search.LazyLoading.loading = true;

                            var nextPageUrl = $("a.nextLink:last").attr('href');

                            if (typeof nextPageUrl != "undefined") {
                                if (Smithsonian.Search.isConsoleDefined) {
                                    console.log('load more', decodeURIComponent(nextPageUrl));
                                }
                                Smithsonian.Search.LazyLoading.loadPage(nextPageUrl);
                            } else {
                                if (Smithsonian.Search.isConsoleDefined) console.log("End of results. Page was not loaded.");
                                Smithsonian.Search.LazyLoading.setEndOfResults();
                            }
                        }
                    });
                }
            }
        },
        
        doRequest: function (url, method, data, query, rememberState) {
            if (Smithsonian.Search.isConsoleDefined) {
                console.log('doRequest', url);
            }

            if (typeof url == "undefined" || url == '') {
                if (Smithsonian.Search.isConsoleDefined) {
                    console.error("URL for Ajax request is undefined or empty.", this);
                    throw "URL for Ajax request is undefined or empty.";
                }
                return;
            }

            var loadingBlock = $(Smithsonian.Search.selectors.loadingBlock);

            method = (typeof method == "undefined" || method.toLowerCase() != 'post') ? "get" : method;

            $(loadingBlock).css("display", "block");

            $.ajax({
                url: url,
                method: method,
                data: data || {},
                cache: true,
                success: function (result) {
                    if (Smithsonian.Search.isConsoleDefined) {
                        console.log('doRequest, success');
                    }

                    if (rememberState !== false) {
                        // Save state (modify URL via History.js)
                        var queryParam = (query != undefined && typeof query == "string") ? "&query=" + encodeURIComponent(query) : "";
                        var parameters = window.location.pathname + url.substr(url.indexOf("?"), url.length) + queryParam;
                        History.pushState(parameters, '', parameters);
                    }
                    
                    Smithsonian.Search.processSearchResults(result);
                },
                error: function (error) {
                    if (Smithsonian.Search.isConsoleDefined) {
                        console.log('doRequest, error', JSON.stringify(error));
                    }

                    $(loadingBlock).css("display", "none");
                }
            });
            
        },
        
        processSearchResults: function (/* HTML string */ result, /* bool */ firstTimeLoad) {
            if (Smithsonian.Search.isConsoleDefined) {
                console.log("processSearchResults", typeof result == "undefined");
            }

            Smithsonian.Search.LazyLoading.setUpLazyLoading(firstTimeLoad);

            var resultNode = $(Smithsonian.Search.selectors.resultContainer);
            var loadingBlock = $(Smithsonian.Search.selectors.loadingBlock);

            // Hide 'loading' screen
            $(loadingBlock).css("display", "none");

            // Display results HTML
            if (typeof result != "undefined" && result != '' && result != null) {
                $(resultNode).html(result);
            }

            // Search form submission
            $(Smithsonian.Search.selectors.searchForm).submit(function () {
                var query = $(Smithsonian.Search.selectors.queryInput, this).val();
                if (query != null && typeof query == 'string' && query.length > 0) {
                    Smithsonian.Search.doRequest.call(this, this.action, this.method, $(this).serialize(), query);
                }
                return false;
            });

            // Clicks that invoke search (filters checkboxes)
            $(Smithsonian.Search.selectors.filterCheckboxes).click(function () {
                var url = $(this).attr('value');
                Smithsonian.Search.doRequest.call(this, url);
            });

            // Links that invoke search (pagination, clear filters link, change type of view, related searches, tabs (desktop view))
            $('a.filter, #displayOptions a.view, a.relatedSearch, #tabs a.tab').click(function () {
                var url = $(this).attr('href');
                Smithsonian.Search.doRequest.call(this, url);
                return false;
            });

            // Load more results link for IE7 (other browsers use lazy loading on scroll)
            $(Smithsonian.Search.selectors.visibleNextLink).click(function () {
                var url = $(this).attr('href');
                Smithsonian.Search.LazyLoading.loadPage.call(this, url);
                return false;
            });

            // Request server if sorting option is changed or another tab (mobile view) is selected
            $(Smithsonian.Search.selectors.dropDownLists).change(function () {
                var url = $(":selected", this).attr("value");
                Smithsonian.Search.doRequest.call(this, url);
                return false;
            });

            // Transform selects into stylable drop down lists with jquery mobile
            $(Smithsonian.Search.selectors.dropDownLists).selectmenu({ inline: true, mini: true });
			
            (function setUpCollapsingExpandingFilters() {
                /*
                * 'See more', 'See less' links for filters
                */

                $('a.seeMoreFilterOptions.first-5-options').click(function () {
                    $(this).find('~ .from-6-to-20-options').css("display", "block");
                    $(this).css("display", "none");
                    return false;
                });

                $('a.seeMoreFilterOptions.from-6-to-20-options').click(function () {
                    $(this).find('~ .from-21-to-40-options').css("display", "block");
                    $(this).css("display", "none");
                    return false;
                });

                $('a.seeLessFilterOptions').click(function () {
                    $(this).siblings('.from-6-to-20-options,.from-21-to-40-options').css('display', 'none');
                    $(this).siblings('a.seeMoreFilterOptions.first-5-options').css('display', '');
                    $(this).css('display', 'none');

                    return false;
                });
            }());
            
            // Modify redirect URL before submitting to shopping cart - add current time in ms. Will be checked before displaying a confirmation popup.
            $(Smithsonian.Search.selectors.buyForm).submit(function () {
                var redirect = $(this).find('input[name=redirect]');
                var url = $(redirect).val();
                var urlParam = "addedtocart=";
                $(redirect).val(url.replace(urlParam, urlParam + (new Date()).getTime()));
            });

            // Select all query text when query textbox is focused to facilitate removing current query text.
            if (!Smithsonian.Search.isWindowsPhone()) {
                $(Smithsonian.Search.selectors.queryInput).click(function () {
                    this.select();
                    var searchBox = document.getElementById(Smithsonian.Search.selectors.queryId);
                    if (typeof searchBox != 'undefined' && typeof searchBox.setSelectionRange != 'undefined') {
                        searchBox.setSelectionRange(0, 9999);
                    }
                });
            }

            (function setUpClearSearchBoxButton() {
                /* Clear search phrase button 'inside'the search box */
                var cssNoImageClass = "noImage";

                var toggleVisibilityCallback = function () {
                    if ($(this).val().length != 0) {
                        $(Smithsonian.Search.selectors.clearSearchBoxLink).removeClass(cssNoImageClass);
                    } else {
                        $(Smithsonian.Search.selectors.clearSearchBoxLink).addClass(cssNoImageClass);
                    }
                };

                var hideCallback = function () {
                    $(Smithsonian.Search.selectors.clearSearchBoxLink).addClass(cssNoImageClass);
                };

                $(Smithsonian.Search.selectors.queryInput).keyup(toggleVisibilityCallback);
                $(Smithsonian.Search.selectors.queryInput).focus(toggleVisibilityCallback);

                $(Smithsonian.Search.selectors.queryInput).blur(hideCallback());
                $(Smithsonian.Search.selectors.searchForm).submit(hideCallback());

                $(Smithsonian.Search.selectors.clearSearchBoxLink).click(function () {
                    $(Smithsonian.Search.selectors.queryInput).val("");
                    $(Smithsonian.Search.selectors.clearSearchBoxLink).addClass(cssNoImageClass);
                    return false;
                });
            })();
            
            (function setUpGsaSearchSuggestions() {
                /*
                * GSA search suggestions
                */

                $(Smithsonian.Search.selectors.queryInput).autocomplete({
                    source: function (request, response) {

                        var pathEndsWithSlash = (window.location.pathname.charAt(window.location.pathname.length - 1) == '/');
                        var path = pathEndsWithSlash ? window.location.pathname : window.location.pathname + '/';

                        $.ajax({
                            url: path + "suggest",
                            dataType: "json",
                            data: {
                                query: $(this.element).val()
                            },
                            success: function (data) {
                                response($.map(data[1], function (item) {
                                    return {
                                        label: item,
                                        value: item
                                    };
                                }));
                            }
                        });
                    }
                });
            })();

			// 'Back to top' button
            Smithsonian.Search.setUpScrollToTop();

            // Initialize and build sharing buttons with AddThis
            (function InitAddThisSharingButtons() {
                if (typeof addthis != 'undefined') {
                    addthis.init();
                    addthis.toolbox(Smithsonian.Search.selectors.addThisToolbox);
                }
            }());

            // Rebbot pickle player
            PKL_Reboot();
        },

        restoreStateForHtml4Browser: function () {
            // HTML4 browser - do request explicitly based on URL hash

            var pathname = window.location.pathname.substr(1, window.location.pathname.length).toLowerCase();
            var searchModelParamName = "jsonsearchmodel";
            var isHtml4 = (typeof window.history == 'undefined' || typeof window.history.pushState == 'undefined');
            var containsJsonSearchModel = (window.location.href.toLowerCase().indexOf(searchModelParamName) != -1);

            if (isHtml4 && containsJsonSearchModel) {
                var state = History.getState();

                if (Smithsonian.Search.isConsoleDefined) { console.log('state hash', state.hash); }

                var searchPath = '/' + pathname + "/search/search";
                searchPath = searchPath.replace('//', '/');

                var index = state.hash.toLowerCase().indexOf("?" + searchModelParamName);
                var paramsString = state.hash.substr(index, state.hash.length);

                var ajaxUrl = searchPath + paramsString;

                if (Smithsonian.Search.isConsoleDefined) console.log("Restore state. ajaxUrl = ", ajaxUrl);
                Smithsonian.Search.doRequest(ajaxUrl, "get", null, null, false);
            }
        },
        
        showAddToCartConfirmation: function () {
            $(Smithsonian.Search.selectors.addTocartPopupMessage).dialog({
                show: { effect: 'fade', duration: 1000 },
                hide: { effect: 'fade', duration: 1000 },
                minHeight: 80,
                open: function () {
                    var element = $(this);
                    setTimeout(function () {
                        element.dialog('close');
                    }, 2000);
                }
            });
            $(".ui-dialog-titlebar").hide();
        },

        howManySecondsSinceAddedToCart: function () {
            var result = Number.MAX_VALUE;

            if (window.location.href.indexOf("&addedtocart=") != -1) {
                var matches = window.location.href.match(/addedtocart=([^&]*)/);
                var savedTimeInMs = matches[1];
                var nowInMs = (new Date()).getTime();
                var diffInSec = ((nowInMs - savedTimeInMs) / 1000);

                result = diffInSec;
            }

            return result;
        },

        setUpScrollToTop: function() {
            var offset = 220;
            var duration = 500;

            var scrollToTop = function () {
                $('body, html').animate({ scrollTop: 0 }, duration);

                // Workaround for Windows Phone
                setTimeout(function () {
                    if (typeof window.scrollTo != 'undefined') {
                        window.scrollTo(0, 0);
                    }
                }, duration + 100);
            };

            var backToTopLink = $(Smithsonian.Search.selectors.backToTopLink);

            $(backToTopLink).click(function (event) {
                event.preventDefault();
                scrollToTop();
                $(this).blur();
                return false;
            });

            $(window).scroll(function () {
                if ($(this).scrollTop() > offset) {
                    $(backToTopLink).fadeIn(duration);
                } else {
                    $(backToTopLink).fadeOut(duration);
                }
            });
        },

        isAnyContentInPlace: function() {
            return ($("#search-results ul.search-results,#search-results table.searchResultsTable").length > 0);
        },

        isWindowsPhone: function() {
            return (navigator.userAgent.match(/Windows Phone/i));
        }
    }
};

$(function () {
    Smithsonian.Search.processSearchResults(null, true);

    Smithsonian.Search.restoreStateForHtml4Browser();
    
    if (Smithsonian.Search.howManySecondsSinceAddedToCart() <= 5) {
        Smithsonian.Search.showAddToCartConfirmation();
    }
if (!Smithsonian.Search.isWindowsPhone()) {
    $('[data-toggle="dropdown"]').dropdownHover();
}
});