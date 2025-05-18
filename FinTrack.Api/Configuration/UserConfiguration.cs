using FinTrack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Api.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Budgets)
            .WithOne(x => x.User);

        builder
            .HasMany(x => x.UserIncomeCategories)
            .WithOne(x => x.User)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.UserExpenseCategories)
            .WithOne(x => x.User);
    }
}
