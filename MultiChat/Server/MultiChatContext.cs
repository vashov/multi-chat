using Microsoft.EntityFrameworkCore;
using MultiChat.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiChat.Server
{
    public class MultiChatContext : DbContext
    {
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }

        public MultiChatContext(DbContextOptions<MultiChatContext> opt) : base(opt)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Invitation>(e =>
            {
                e.HasMany(e => e.InvitedUsers)
                .WithOne(u => u.Invitation)
                .HasForeignKey(e => e.InvitationId);

                e.HasOne(e => e.Owner)
                .WithMany(e => e.InvitationsCreated)
                .HasForeignKey(e => e.OwnerId);

                e.HasOne(e => e.Room)
                .WithMany(e => e.Invitations)
                .HasForeignKey(e => e.RoomId);
            });

            builder.Entity<Room>(e =>
            {
                e.HasOne(e => e.Owner)
                .WithOne(e => e.RoomCreated)
                .HasForeignKey<User>(e => e.RoomCreatedId);

                e.HasMany(e => e.Users)
                .WithOne(e => e.Room)
                .HasForeignKey(e => e.RoomId);

                e.HasMany(e => e.Invitations)
                .WithOne(e => e.Room)
                .HasForeignKey(e => e.RoomId);
            });

            builder.Entity<User>(e =>
            {
                e.HasOne(e => e.RoomCreated)
                .WithOne(e => e.Owner)
                .HasForeignKey<Room>(e => e.OwnerId);

                e.HasOne(e => e.Room)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoomId);

                e.HasOne(e => e.Invitation)
                .WithMany(e => e.InvitedUsers)
                .HasForeignKey(e => e.InvitationId);

                e.HasMany(e => e.InvitationsCreated)
                .WithOne(e => e.Owner)
                .HasForeignKey(e => e.OwnerId);
            });
        }
    }
}
