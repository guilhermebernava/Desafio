using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using qodeless.desafio.domain.Enums;
using System;

namespace qodeless.desafio.domain.Entities
{
    public class Ubuntu : Entity, IEntityTypeConfiguration<Ubuntu>
    {
        public string Name { get; set; }
        public string Telephone { get; set; }
        public DateTime Date { get; set; }
        public string Key { get; set; }
        public  EIndicadorArea IndicatorArea {get;set;}

        public Ubuntu() : base(){}

        public void Configure(EntityTypeBuilder<Ubuntu> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Key).IsRequired();
            builder.Property(x => x.Telephone).IsRequired();
            builder.Property(x => x.Name).IsRequired();
        }
    }
}
