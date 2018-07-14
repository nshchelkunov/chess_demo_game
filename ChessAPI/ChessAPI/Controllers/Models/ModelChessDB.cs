namespace ChessAPI.Controllers.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelChessDB : DbContext
    {
        public ModelChessDB()
            : base("name=ModelChessDB")
        {
        }

        public virtual DbSet<Games> Games { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Games>()
                .Property(e => e.FEN)
                .IsUnicode(false);

            modelBuilder.Entity<Games>()
                .Property(e => e.Status)
                .IsUnicode(false);
        }
    }
}
