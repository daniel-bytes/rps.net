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

    function HandleMoveResult(r) {
        if (r.Message) {
            alert(r.Message);
        }

        var token = _.find($scope.tokens, function (t) { return t.id === r.AttackerTokenID; });

        if (token) {
            token.x = r.MoveToX;
            token.y = r.MoveToY;
        }

        if (tokenService.isGameEndingMove(r.Result)) {
            $window.location.href = "/game";
        }
    }

    $scope.$on('gameboard.move', function (evt, data) {
        gameService.move(data.attacker.id, $scope.gameid, data.defender.x, data.defender.y, function (moveResult) {
            console.log(moveResult.Results);

            HandleMoveResult(moveResult.Results[0]);

            setTimeout(function () {
                if (moveResult.Results.length > 1) {
                    HandleMoveResult(moveResult.Results[1]);
                }

                LoadTokens();
            }, 1000);
        });
    });

    LoadTokens();
}
