﻿function GameResource($resource) {
    return $resource('/api/game/:id');
}

function GameMoveResource($resource) {
    return $resource('/api/game/:id/move', { id: '@GameID' });
}

function GameComputerMoveResource($resource) {
    return $resource('/api/game/:id/computermove', { id: '@GameID' });
}

function GameService(tokenService, gameResource, gameMoveResource, gameComputerMoveResource) {
    return {
        transform: function(data) {
            var rows = [];

            for (var r = 0; r < data.Rows; r++) {
                var row = [];
                for (var c = 0; c < data.Cols; c++) {
                    var token = _(data.Tokens).find(function (x) { return x.Row === r && x.Col === c; });
                    var tokenType = tokenService.tokenTypes.None;

                    if (token) {
                        tokenType = tokenService.getTokenTypeByID(token.TokenType);
                    }

                    row.push({ id: token ? token.TokenID : 0, x: r, y: c, type: tokenType });
                }
                rows.push(row);
            }

            return rows;
        },

        load: function (id, callback) {
            var $this = this;

            return gameResource.get({ id: id }, function (data) {
                var rows = $this.transform(data);

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
                callback(data);
            }, function (err) {
                console.log(err);
                var message = err.data && err.data.ExceptionMessage ? err.data.ExceptionMessage : "An error has occurred";
                alert(message);
            });
        },

        computerMove: function (gameid, callback) {
            var self = this;

            var model = {
                GameID: gameid
            };
            return gameComputerMoveResource.save(model, function (data) {
                callback(data);
            }, function (err) {
                console.log(err);
                var message = err.data && err.data.ExceptionMessage ? err.data.ExceptionMessage : "An error has occurred";
                alert(message);
            });
        }
    };
}
