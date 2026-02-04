using booker.api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace booker.api.Data.Configurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            // 設定主鍵
            builder.HasKey(m => m.Id);

            // 設定 Name 屬性
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            // 設定 Description 屬性
            builder.Property(m => m.Description)
                .HasMaxLength(500);

            // 設定 Category 屬性
            builder.Property(m => m.Category)
                .IsRequired()
                .HasMaxLength(50);

            // 設定 SpecialTag 屬性
            builder.Property(m => m.SpecialTag)
                .HasMaxLength(50);

            // 設定 Price 屬性 (使用 decimal 類型更精確)
            builder.Property(m => m.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // 設定 Image 屬性
            builder.Property(m => m.Image)
                .HasMaxLength(500);

            // 設定索引以提升查詢效能
            builder.HasIndex(m => m.Category);
            builder.HasIndex(m => m.Name);
        }
    }
}
