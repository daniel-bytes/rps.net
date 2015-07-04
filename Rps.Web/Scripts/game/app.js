angular.module('rps', ['ngResource'])
    .controller('game-controller', ['$scope', 'gameService', 'tokenService', '$window', GameController])
    .directive('gameboard', GameBoardDirective)
    .factory('gameResource', ['$resource', GameResource])
    .factory('gameMoveResource', ['$resource', GameMoveResource])
    .factory('gameService', ['tokenService', 'gameResource', 'gameMoveResource', GameService])
    .factory('tokenService', TokenService);
