angular.module('rps', ['ngResource'])
    .controller('game-controller', GameController)
    .directive('gameboard', GameBoardDirective)
    .factory('gameResource', GameResource)
    .factory('gameMoveResource', GameMoveResource)
    .factory('gameService', GameService)
    .factory('tokenService', TokenService);
