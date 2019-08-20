var app;
(function () {
    app = angular.module("CrudModule", []);
    initiateCommonDirectives(app);
})();
app.service('CrudService', function ($rootScope, $http) {
    initiateCommonFunctions($rootScope, $http);
    initiateCommonApiFunctions(this, $http, "/api/Tank");
});
app.controller('CrudController', function ($scope, CrudService) {
    $scope.clear = function () {
        $scope.IsNewRecord = 1;
        $scope.Tank = {
            ID: 0,
            Code: '',
            Name: '',
            Description: '',
            NumCompartment: '',
            Keys: [{
                Key: ''
            }]
        }
    };
    function loadRecords() {
        $scope.clear();
        var promiseGet = CrudService.getAll();
        promiseGet.then(function (pl) {
            $scope.DataBundle = pl.data
        },
              function (errorPl) {
                  $log.error('failure loading Record', errorPl);
              });
        if ($scope.Message != undefined)
            alert($scope.Message);
    };
    loadRecords();
    $scope.save = function () {
        var res = $scope.Tank;
        if ($scope.IsNewRecord === 1) {
            var promisePost = CrudService.post(res);
            promisePost.then(function (pl) {
                $scope.Tank.ID = pl.data.ID;
                $scope.Message = "Record Saved Successfuly";
                loadRecords();
                $scope.IsNewRecord = 0;
            }, function (err) {
                console.log("Error Occcured in Saving Record" + err);
            });
        } else {
            var promisePut = CrudService.put(res.ID, res);
            promisePut.then(function (pl) {
                $scope.Message = "Record Updated Successfuly";
                loadRecords();
                $scope.IsNewRecord = 0;
            }, function (err) {
                console.log("Error Occcured in Updating Record" + err);
            });
        }
    };
    $scope.delete = function () {
        var promiseDelete = CrudService.delete($scope.Tank.ID);
        promiseDelete.then(function (pl) {
            $scope.Message = "Record Deleted Successfuly";
            $scope.clear();
            loadRecords();
        }, function (err) {
            console.log("Err" + err);
        });
    };
    $scope.get = function (Record) {
        $scope.clear();
        var promiseGetSingle = CrudService.get(Record.ID);
        promiseGetSingle.then(function (pl) {
            var res = pl.data;
            $scope.Tank = {
                ID: res.ID,
                Code: res.Code,
                Name: res.Name,
                Description: res.Description,
                NumCompartment: res.NumCompartment,
                Keys: res.Keys
            }
            $scope.IsNewRecord = 0;
        },
                  function (errorPl) {
                      console.log('failure in loading Record', errorPl);
                  });
    };
});






