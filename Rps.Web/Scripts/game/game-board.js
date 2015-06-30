function GameBoardDirective() {
    return {
        restrict: 'AE',
        replace: 'true',
        template: '<table class="gameboard"></table>',
        link: function ($scope, elem, attrs) {
            console.log("GameBoardDirective Begin");
            var table = elem[0];
            var activeToken = null;

            function CanSelect(token) {
                if (!activeToken) {
                    return false;
                }
                else if (activeToken.type === $scope.tokenService.tokenTypes.Flag || activeToken.type === $scope.tokenService.tokenTypes.Bomb) {
                    return false;
                }

                if (token.x === activeToken.x) {
                    if (token.y === (activeToken.y - 1) || token.y === (activeToken.y + 1)) {
                        return true;
                    }
                }
                else if (token.y === activeToken.y) {
                    if (token.x === (activeToken.x - 1) || token.x === (activeToken.x + 1)) {
                        return true;
                    }
                }

                return false;
            }

            function CellClick(token) {
                activeToken = token;

                $scope.tokens.forEach(function (r) {
                    r.forEach(function (c) {
                        c.active = (c == activeToken);
                    });
                });

                for (var r = 0; r < table.rows.length; r++) {
                    var tr = table.rows[r];
                    var cells = tr.childNodes;

                    for (var c = 0; c < cells.length; c++) {
                        var td = cells[c];
                        var currentToken = $scope.tokens[r][c];

                        if (currentToken.type.id === $scope.tokenService.tokenTypes.OtherPlayer.id) {
                            if (CanSelect(currentToken)) {
                                td.className = "gameboard-cell gameboard-cell-other gameboard-cell-selectable";
                            }
                            else {
                                td.className = "gameboard-cell gameboard-cell-other";
                            }
                        }
                        else if (currentToken.type.id == $scope.tokenService.tokenTypes.None.id) {
                            if (CanSelect(currentToken)) {
                                td.className = "gameboard-cell gameboard-cell-selectable";
                            }
                            else {
                                td.className = "gameboard-cell";
                            }
                        }
                        else {
                            if (currentToken == activeToken) {
                                td.className = "gameboard-cell gameboard-cell-active";
                            }
                            else {
                                td.className = "gameboard-cell gameboard-cell-player";
                            }
                        }
                    }
                }
            }

            function CellRelease() {
                activeToken = null;
                var result = false;

                for (var r = 0; r < table.rows.length; r++) {
                    var tr = table.rows[r];
                    var cells = tr.childNodes;

                    for (var c = 0; c < cells.length; c++) {
                        var td = cells[c];
                        var cell = $scope.tokens[r][c];
                        cell.active = false;

                        if (cell.type.id === $scope.tokenService.tokenTypes.OtherPlayer.id) {
                            td.className = "gameboard-cell gameboard-cell-other";
                        }
                        else if (cell.type.id == $scope.tokenService.tokenTypes.None.id) {
                            td.className = "gameboard-cell";
                        }
                        else {
                            td.className = "gameboard-cell gameboard-cell-player";
                        }
                    }
                }

                return result;
            }

            function BuildTable() {
                table.innerHTML = "";

                table.addEventListener('mouseleave', function (e) {
                    CellRelease();
                });

                $scope.tokens.forEach(function (row, rowidx) {
                    var tr = table.insertRow(rowidx);
                    row.forEach(function (token, cellidx) {
                        var td = tr.insertCell(cellidx);
                        td.innerHTML = token.type.name;

                        if (token.type.id === $scope.tokenService.tokenTypes.OtherPlayer.id) {
                            td.className = "gameboard-cell gameboard-cell-other";
                        }
                        else if (token.type.id == $scope.tokenService.tokenTypes.None.id) {
                            td.className = "gameboard-cell";
                        }
                        else {
                            td.className = "gameboard-cell gameboard-cell-player";

                            td.addEventListener('mousedown', function (e) {
                                CellClick(token, td);
                            });
                        }

                        td.addEventListener('mouseup', function (e) {
                            if (CanSelect(token)) {
                                $scope.$emit('gameboard.move', { attacker: activeToken, defender: token });
                            }
                            CellRelease();
                        });
                    });
                });
            }

            $scope.$watch('tokens', function (value) {
                BuildTable();
            });
        }
    };
}