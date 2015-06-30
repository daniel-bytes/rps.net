function GameResource($resource) {
    return $resource('/api/game/:id');
}

function GameMoveResource($resource) {
    return $resource('/api/game/:id/move', { id: '@GameID' });
}

function GameService(tokenService, gameResource, gameMoveResource) {
    return {
        load: function (id, callback) {
            return gameResource.get({ id: id }, function (data) {
                var rows = [];

                for (var r = 0; r < data.Rows; r++) {
                    var row = [];
                    for (var c = 0; c < data.Cols; c++) {
                        var token = _(data.Tokens).find(function(x) { return x.Row === r && x.Col === c; });
                        var tokenType = tokenService.tokenTypes.None;

                        if (token) {
                            tokenType = tokenService.getTokenTypeByID(token.TokenType);
                        }

                        row.push({ id: token ? token.TokenID : 0, x: r, y: c, type: tokenType });
                    }
                    rows.push(row);
                }
                console.log(rows);
                callback(rows);
            });
        },

        move: function (id, gameid, x, y, callback) {
            var self = this;

            var model = {
                GameID: gameid,
                TokenID: id,
                MoveToX: x,
                MoveToY: y
            };
            return gameMoveResource.save(model, function (data) {
                switch (data.Result) {
                    case tokenService.moveResultTypes.TokenMove.id:
                    case tokenService.moveResultTypes.AttackerWins.id:
                    case tokenService.moveResultTypes.DefenderWins.id:
                    case tokenService.moveResultTypes.BothLose.id:
                        self.load(gameid, callback);
                        break;
                    case tokenService.moveResultTypes.GameOver.id:
                        // TODO!
                        alert("Game Over!");
                        break;
                }
            }, function (err) {
                console.log(err);
                var message = err.data.ExceptionMessage ? err.data.ExceptionMessage : "An error has occurred";
                alert(message);
            });
        }
    };
}
