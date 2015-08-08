function GameController($scope, gameService, tokenService, $window) {
    $scope.gameid = $window.gameid;
    $scope.tokens = [];
    $scope.tokenService = tokenService;
    console.log("gameid: %s", $scope.gameid);

    function LoadTokens() {
        gameService.load($scope.gameid, function (data) {
            $scope.tokens = data;
            $scope.$emit("refresh");
        });
    }

    function HandleMoveResult(r) {
        $scope.tokens = gameService.transform(r);
        $scope.$emit("refresh");

        if (r.Message) {
            alert(r.Message);
        }

        if (tokenService.isGameEndingMove(r.Result)) {
            $window.location.href = "/game";
        }
    }

    $scope.$on('gameboard.move', function (evt, data) {
        console.log("Player move");
        gameService.move(data.attacker.id, $scope.gameid, data.defender.x, data.defender.y, function (moveResult) {
            
            HandleMoveResult(moveResult.Results[0]);

            $scope.locked = true;

            console.log("computer move");
            gameService.computerMove($scope.gameid, function (computerMoveResult) {
                console.log(computerMoveResult.Results);
                
                setTimeout(function () {
                    HandleMoveResult(computerMoveResult.Results[0]);
                    $scope.locked = false;
                    console.log("turn complete.");
                }, 500);

            });
        });
    });

    LoadTokens();
}
