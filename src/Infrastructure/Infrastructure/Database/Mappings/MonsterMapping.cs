using Domain.Monsters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mappings;

public class MonsterMapping : IEntityTypeConfiguration<Monster>
{
    public void Configure(EntityTypeBuilder<Monster> builder)
    {
        builder.ToTable("Monster");

        builder.Property(p => p.Id).HasColumnType("INTEGER").IsRequired().ValueGeneratedOnAdd();
        builder.Property(p => p.Attack).HasColumnType("INTEGER").IsRequired();
        builder.Property(p => p.Defense).HasColumnType("INTEGER").IsRequired();
        builder.Property(p => p.Hp).HasColumnType("INTEGER").IsRequired();
        builder.Property(p => p.Name).HasColumnType("TEXT").IsRequired();
        builder.Property(p => p.ImageUrl).HasColumnType("TEXT").IsRequired();
        builder.Property(p => p.Speed).HasColumnType("INTEGER").IsRequired();

        builder.HasKey(p => p.Id);

        //builder.HasMany<Battle>().WithOne(c => c.MonsterARelation).HasForeignKey(c => c.MonsterA).HasPrincipalKey(c => c.Id);
        //builder.HasMany<Battle>().WithOne(c => c.MonsterBRelation).HasForeignKey(c => c.MonsterB).HasPrincipalKey(c => c.Id);
        //builder.HasMany<Battle>().WithOne(c => c.WinnerRelation).HasForeignKey(c => c.Winner).HasPrincipalKey(c => c.Id);

        builder.HasData(new Monster[]
        {
            new Monster
            (
                new Guid(),
                "Dead Unicorn",
                60,
                40,
                10,
                "https://fsl-assessment-public-files.s3.amazonaws.com/assessment-cc-01/dead-unicorn.png",
                80
            ),
            new Monster
            (
                new Guid(),
                "Old Shark",
                50,
                20,
                80,
                "https://fsl-assessment-public-files.s3.amazonaws.com/assessment-cc-01/old-shark.png",
                90
            ),
            new Monster
            (
                new Guid(),
                "Red Dragon",
                90,
                80,
                90,
                "https://fsl-assessment-public-files.s3.amazonaws.com/assessment-cc-01/red-dragon.png",
                70
            ),
            new Monster
            (
                new Guid(),
                "Robot Bear",
                50,
                40,
                80,
                "https://fsl-assessment-public-files.s3.amazonaws.com/assessment-cc-01/robot-bear.png",
                60
            ),
            new Monster(
                new Guid(),
                "Angry Snake",
                80,
                20,
                70,
                "https://fsl-assessment-public-files.s3.amazonaws.com/assessment-cc-01/angry-snake.png",
                80
            )
        });
    }
}