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

        moveResultTypes: {
            Nothing: { id: 0 },
            TokenMove: { id: 1 },
            AttackerWins: { id: 2 },
            DefenderWins: { id: 3 },
            BothLose: { id: 4 },
            GameOver: { id: 5 }
        }
    };
}
