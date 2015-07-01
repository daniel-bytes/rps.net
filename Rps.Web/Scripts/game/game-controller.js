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
        gameService.move(data.attacker.id, $scope.gameid, data.defender.x, data.defender.y, function (moveResult) {
            console.log(moveResult);

            if (moveResult.Message) {
                alert(moveResult.Message);
            }

            if (moveResult.Result === tokenService.moveResults.GameOver) {
                $window.location.href = "/game";
            }
            else {
                LoadTokens();
            }
        });
    });

    LoadTokens();
}
