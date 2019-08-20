

var ZoneFormat = "ZD";
var DefaultOffset = "+00:00";
var DateFormat = "DD-MM-YYYY";
var TimeFormat = "hh:mm a";
var ISOFormat = "YYYY-MM-DDTHH:mm:ssZD";
var SerializableFormat = "YYYY-MM-DDTHH:mm:ss";
var DateTimeFormat = DateFormat + " " + TimeFormat;
var DateTimeFormatWithZone = DateTimeFormat + ZoneFormat;

function makeError(errObj, preText, postText) {
    return (preText || "") +
        " \n Status: " + errObj.status + " " + errObj.statusText +
        " \n URL : " + errObj.config.url +
        " \n Method : " + errObj.config.method +
        " \n Message: " + errObj.data.Message + " \n " +
        (postText || "");
}
function initiateCommonFunctions($scope, $http) {
    var ListURL = '/api/commonapi/Get';
    $scope.getList = function (ListName) {
        var promiseGet = $http.get(ListURL + ListName);
        promiseGet.then(function (pl) {
            $scope[ListName] = pl.data;
        },
        function (errorPl) {
            console.log(makeError(errorPl, ListName + ' load failed.'));
        });
    }
    $scope.getLists = function (ListName) {
        $.each(ListName, function (idx, lst) {
            var promiseGet = $http.get(ListURL + lst);
            promiseGet.then(function (pl) {
                $scope[lst] = pl.data;
            },
            function (errorPl) {
                console.log(makeError(errorPl, lst + ' load failed.'));
            });
        });
    }
 
    $scope.getProductDetailList = function (obj) {
        var promiseGet = $http.get("/api/commonapi/GetProductDetailListById/" + obj.Records.ProductID);
        promiseGet.then(function (pl) {
            obj.ProductDetailList = pl.data;
        },
       function (errorPl) {
           console.log(makeError(errorPl, 'List load failed.'));
       });
    }
   

}
function initiateCommonDirectives(app) {
    function fromUser(text) {
        var val = ((text || '') == '' ? '' : moment(text + DefaultOffset, DateTimeFormatWithZone).zone(DefaultOffset).format(SerializableFormat));
        return val;
    }
    function toUser(text) {
        var val = ((text || '') == '' ? '' : moment(text + DefaultOffset, ISOFormat).zone(DefaultOffset).format(DateTimeFormat));
        return val;
    }
    var opts = {
        format: DateTimeFormat,
        allowInputToggle: true,
        toolbarPlacement: 'top',
        icons: {
            time: 'fa fa-clock-o',
            date: 'fa fa-calendar',
            up: 'fa fa-chevron-up',
            down: 'fa fa-chevron-down',
            previous: 'fa fa-chevron-left',
            next: 'fa fa-chevron-right',
            today: 'fa fa-screenshot',
            clear: 'fa fa-trash',
            close: 'fa fa-remove'
        }

    }
    app.directive('chosen', function () {
        var linker = function (scope, element, attr) {
            // update the select when data is loaded
            scope.$watch(attr.chosen, function (oldVal, newVal) {
                $(element).trigger('chosen:updated');
            });

            // update the select when the model changes
            scope.$watch(attr.ngModel, function () {
                $(element).trigger('chosen:updated');
            });

            $(element).chosen().change(function (event, params) {
                var model = scope;
                var prop = "";
                var arr = attr.ngModel.split('.');
                $.each(arr, function (idx, str) {
                    if (idx < (arr.length - 1))
                        model = scope[str] || {};
                    else
                        prop = str;
                });
                model[prop] = scope[attr.chosen][$(this).val()].Value || scope[attr.chosen][$(this).val()].ID;
                scope.$digest();
            });
        };

        return {
            restrict: 'A',
            link: linker
        };
    })
        .directive('datatable', function () {
            return function (scope, element, attrs) {
                scope.$watch("assignments", function (value) {
                    if (scope.$last) {
                        $(element).parent().parent().tablesorter();
                        $(element).parent().parent().dataTable({ retrieve: true });
                    }
                });
            };
        })
        .directive('datetime', function () {
            return {
                restrict: 'A',
                require: 'ngModel',
                link: function ($scope, element, attr, ngModel) {
                    
                    ngModel.$parsers.push(fromUser);
                    ngModel.$formatters.push(toUser);

                    $(element).parent().datetimepicker(opts).on('dp.change', function (e) {
                        var ele = $(element);
                        var model = $scope;
                        var status = true;
                        var prop = "";
                        var arr = attr.ngModel.split('.');
                        $.each(arr, function (idx, str) {
                            if (idx < (arr.length - 1))
                                model = $scope[str] || {};
                            else
                                prop = str;
                        });
                        $scope.$apply(function () {
                            model[prop] = moment(ele.val() + DefaultOffset, DateTimeFormatWithZone).zone(DefaultOffset).format(SerializableFormat);
                        });
                    });
                }
            };
        })
    //blur directive
    //.directive('myBlur', function () {
    //    return {
    //        restrict: 'A',
    //        link: function (scope, element, attr) {
    //            element.bind('blur', function () {
    //                //apply scope (attributes)
    //                scope.$apply(attr.myBlur);
    //                //return scope value for focusing to false
    //                scope.$eval(attr.myFocus + '=false');
    //            });
    //        }
    //    };
    //})
    //.directive('ngBlur', ['$parse', function ($parse) {
    //    return function (scope, element, attr) {
    //        var fn = $parse(attr['ngBlur']);
    //        element.bind('blur', function (event) {
    //            scope.$apply(function () {
    //                fn(scope, { $event: event });
    //            });
    //        });
    //    }
    //}])
    //.directive('uiBlur', function () {
    //    return function (scope, elem, attrs) {
    //        elem.bind('blur', function () {
    //            scope.$apply(attrs.uiBlur);
    //        });
    //    }
    //})
    .directive('checkImage', function ($http) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                attrs.$observe('ngSrc', function (ngSrc) {
                    $http.get(ngSrc).success(function () {
                        //alert('image exist');
                    }).error(function () {
                       // alert('image not exist');
                        element.attr('src', '../../Content/assets/img/no_image_small.png'); // set default image
                    });
                });
            }
        };
    });
}
function initiateCommonApiFunctions(service, $http, url) {
    var token = sessionStorage.getItem("accessToken");
    $http.defaults.headers.common.Authorization = 'Bearer ' + token;

    function exceptionHandler(errObj) {
        console.log(makeError(errObj));
    }

    service.post = function (DataBundle) {
        var request = $http({
            method: "post",
            url: url,
            data: DataBundle
        }).catch(exceptionHandler);
        return request;
    }
    service.get = function (ID) {
        return $http.get(url + "/" + ID).catch(exceptionHandler);
    }
    service.getAll = function () {
        return $http.get(url).catch(exceptionHandler);
    }
    service.put = function (ID, DataBundle) {
        var request = $http({
            method: "put",
            url: url + "/" + ID,
            data: DataBundle
        }).catch(exceptionHandler);
        return request;
    }
    service.delete = function (ID) {
        var request = $http({
            method: "delete",
            url: url + "/" + ID
        }).catch(exceptionHandler);
        return request;
    }
}