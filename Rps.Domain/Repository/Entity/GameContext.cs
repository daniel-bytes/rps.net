using Rps.Domain.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Repository.Entity
{
    public class GameContext
        : DbContext, IGameContext
    {
        public const string IX_Game_Active = "IX_Game_Active";

        public GameContext(IDomainConfig config)
            : base(config.DefaulConnectionString)
        {
            this.Init();
        }

        public GameContext()
            : base(DomainConfig.ConnStringKey_DefaultConnection)
        {
            this.Init();
        }

        public IDbSet<Game> Games { get; set; }
        public IDbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Configure_IX_Game_Active(modelBuilder);
        }

        protected void Init()
        {
            this.Database.Log = q => System.Diagnostics.Debug.WriteLine(q);
        }

        protected void Configure_IX_Game_Active(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .Property(e => e.Active)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute(IX_Game_Active, 1)));
        }
    }
}
