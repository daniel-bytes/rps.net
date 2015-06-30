function GameController($scope, gameService, tokenService, $window) {
    $scope.gameid = $window.gameid;
    $scope.tokens = [];
    $scope.tokenService = tokenService;
    console.log("gameid: %s", $scope.gameid);

    function LoadTokens() {
        gameService.load($scope.gameid, function (data) {
            $scope.tokens = data;
        });
    }

    $scope.$on('gameboard.move', function (evt, data) {
        console.log('gameboard.move handler:');
        console.log(data.attacker);
        console.log(data.defender);
        gameService.move(data.attacker.id, $scope.gameid, data.defender.x, data.defender.y, function (result) {

        });
    });

    LoadTokens();
}
