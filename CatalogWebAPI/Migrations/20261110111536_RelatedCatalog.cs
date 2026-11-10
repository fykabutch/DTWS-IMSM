using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogWebAPI.Migrations;

public partial class RelatedCatalog : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            ALTER TABLE Specialties RENAME TO LegacySpecialties;
            CREATE TABLE Directions (
                Id INTEGER NOT NULL CONSTRAINT PK_Directions PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL
            );
            CREATE TABLE Specialties (
                Id INTEGER NOT NULL CONSTRAINT PK_Specialties PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Code TEXT NOT NULL,
                DirectionId INTEGER NOT NULL,
                CONSTRAINT FK_Specialties_Directions_DirectionId FOREIGN KEY (DirectionId)
                    REFERENCES Directions (Id) ON DELETE RESTRICT
            );
            CREATE TABLE Specializations (
                Id INTEGER NOT NULL CONSTRAINT PK_Specializations PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                SpecialtyId INTEGER NOT NULL,
                CONSTRAINT FK_Specializations_Specialties_SpecialtyId FOREIGN KEY (SpecialtyId)
                    REFERENCES Specialties (Id) ON DELETE RESTRICT
            );
            CREATE INDEX IX_Specialties_DirectionId ON Specialties (DirectionId);
            CREATE INDEX IX_Specializations_SpecialtyId ON Specializations (SpecialtyId);

            INSERT INTO Directions (Name)
                SELECT DISTINCT CASE WHEN trim(FieldOfStudy) = '' THEN 'Без направления'
                    ELSE trim(FieldOfStudy) END FROM LegacySpecialties;
            INSERT INTO Specialties (Id, Name, Code, DirectionId)
                SELECT old.Id, old.EducationalProgram, '', d.Id
                FROM LegacySpecialties old JOIN Directions d
                ON d.Name = CASE WHEN trim(old.FieldOfStudy) = '' THEN 'Без направления'
                    ELSE trim(old.FieldOfStudy) END;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        throw new NotSupportedException("Обратная миграция требует ручного переноса данных. Исходные записи сохранены в LegacySpecialties.");
    }
}

