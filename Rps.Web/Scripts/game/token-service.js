function TokenService() {
    return {
        tokenTypes: {
            None: { id: 0, name: "" },
            Rock: { id: 1, name: "Rock" },
            Paper: { id: 2, name: "Paper" },
            Scissor: { id: 3, name: "Scissor" },
            Bomb: { id: 4, name: "Bomb" },
            Flag: { id: 5, name: "Flag" },
            OtherPlayer: { id: 100, name: "X" }
        },

        moveResults: {
            Nothing: 0,
            TokenMove: 1,
            AttackerWins: 2,
            DefenderWins: 3,
            BothLose: 4,
            FlagCaptured: 5,
            AttackerOutOfPieces: 6,
            DefenderOutOfPieces: 7,
            BothOutOfPieces: 8
        },

        getTokenTypeByID: function (id) {
            switch (id) {
                case 0: return this.tokenTypes.None;
                case 1: return this.tokenTypes.Rock;
                case 2: return this.tokenTypes.Paper;
                case 3: return this.tokenTypes.Scissor;
                case 4: return this.tokenTypes.Bomb;
                case 5: return this.tokenTypes.Flag;
                case 100: return this.tokenTypes.OtherPlayer;
            }
        },

        isGameEndingMove: function(id) {
            return id === this.moveResults.FlagCaptured ||
                id === this.moveResults.AttackerOutOfPieces ||
                id === this.moveResults.DefenderOutOfPieces ||
                id === this.moveResults.BothOutOfPieces;
        }
    };
}
