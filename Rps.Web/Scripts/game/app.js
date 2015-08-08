angular.module('rps', ['ngResource'])
    .controller('game-controller', ['$scope', 'gameService', 'tokenService', '$window', GameController])
    .directive('gameboard', GameBoardDirective)
    .factory('gameResource', ['$resource', GameResource])
    .factory('gameMoveResource', ['$resource', GameMoveResource])
    .factory('gameComputerMoveResource', ['$resource', GameComputerMoveResource])
    .factory('gameService', ['tokenService', 'gameResource', 'gameMoveResource', 'gameComputerMoveResource', GameService])
    .factory('tokenService', TokenService);
