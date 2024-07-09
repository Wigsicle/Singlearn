﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Singlearn.Data;

#nullable disable

namespace Singlearn.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240708093135_AddHomeworkEntitty")]
    partial class AddHomeworkEntitty
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Singlearn.Models.Entities.Announcement", b =>
                {
                    b.Property<int>("announcement_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("announcement_id"));

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("message_body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("subject_id")
                        .HasColumnType("int");

                    b.Property<string>("teacher_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("announcement_id");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("Singlearn.Models.Entities.ChapterName", b =>
                {
                    b.Property<int>("chapter_name_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("chapter_name_id"));

                    b.Property<int>("chapter_id")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("subject_id")
                        .HasColumnType("int");

                    b.HasKey("chapter_name_id");

                    b.ToTable("ChapterNames");
                });

            modelBuilder.Entity("Singlearn.Models.Entities.Class", b =>
                {
                    b.Property<string>("class_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("academic_level")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("teacher_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("year")
                        .HasColumnType("int");

                    b.HasKey("class_Id");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("Singlearn.Models.Entities.Material", b =>
                {
                    b.Property<int>("material_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("material_id"));

                    b.Property<int>("chapter_id")
                        .HasColumnType("int");

                    b.Property<string>("class_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("subject_id")
                        .HasColumnType("int");

                    b.Property<string>("teacher_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("material_id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Singlearn.Models.Entities.STCTemplate", b =>
                {
                    b.Property<int>("stc_t_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("stc_t_id"));

                    b.Property<int>("stc_id")
                        .HasColumnType("int");

                    b.Property<int>("template_id")
                        .HasColumnType("int");

                    b.HasKey("stc_t_id");

                    b.ToTable("STCTemplates");
                });

            modelBuilder.Entity("Singlearn.Models.Entities.Staff", b =>
                {
                    b.Property<string>("staff_id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("contact_no")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("staff_id");

                    b.ToTable("Staff");
                });

            modelBuilder.Entity("Singlearn.Models.Entities.Student", b =>
                {
                    b.Property<string>("student_id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("class_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("contact_no")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("student_id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Singlearn.Models.Entities.Subject", b =>
                {
                    b.Property<int>("subject_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("subject_id"));

                    b.Property<string>("academic_level")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("no_chapters")
                        .HasColumnType("int");

                    b.Property<int>("year")
                        .HasColumnType("int");

                    b.HasKey("subject_id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Singlearn.Models.Entities.SubjectTeacherClass", b =>
                {
                    b.Property<int>("stc_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("stc_id"));

                    b.Property<string>("class_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("subject_id")
                        .HasColumnType("int");

                    b.Property<string>("teacher_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("stc_id");

                    b.ToTable("SubjectTeacherClasses");
                });

            modelBuilder.Entity("Singlearn.Models.Entities.Template", b =>
                {
                    b.Property<int>("template_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("template_id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("view_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("template_id");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("Singlearn.Models.Entities.User", b =>
                {
                    b.Property<Guid>("user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("user_id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
