using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;

namespace EfCoreRelationships.Models
{
    public partial class EfCoreRelationshipsDbContext : DbContext
    {
        public EfCoreRelationshipsDbContext(DbContextOptions<EfCoreRelationshipsDbContext> options)
            :base(options)
        {
            
        }

        public virtual DbSet<User> Users { get; set; } = default!;
        public virtual DbSet<Blog> Blogs { get; set; } = default!;
        public virtual DbSet<Post> Posts { get; set; } = default!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pgcrypto")
                .HasPostgresExtension("uuid-ossp")
                .HasAnnotation("Relational:Collation", "en_US.utf8");
            
            modelBuilder.HasSequence("global_id_seq");

            // one to one
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.Id)
                    .HasName("pk_users_id");
                
                entity.HasIndex(e => e.Account, "unq_users_account")
                    .IsUnique();
                
                
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Account).HasColumnName("account")
                    .IsRequired()
                    .HasMaxLength(20);
                
                entity.Property(e => e.Password).HasColumnName("password")
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Created).HasColumnName("created")
                    .HasColumnType("timestamp(6) with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(p => p.Profile)
                    .WithOne(u => u.User)
                    .HasForeignKey<Profile>(p => p.UserId);
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.UserId);
            });

            // one to many
            modelBuilder.Entity<Box>(entity =>
            {
            });

            modelBuilder.Entity<BoxItem>(entity =>
            {
                entity.HasOne(bt => bt.Box)
                    .WithMany(b => b.Items)
                    .HasForeignKey(bt=> bt.BoxId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            
            OnModelCreatingPartial(modelBuilder); 
        }
        
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}