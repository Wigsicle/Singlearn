﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Singlearn.Data;

#nullable disable

namespace Singlearn.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("class_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<string>("url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("announcement_id");

                    b.ToTable("Announcements", (string)null);
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

                    b.ToTable("ChapterNames", (string)null);
                });

            modelBuilder.Entity("Singlearn.Models.Entities.Class", b =>
                {
                    b.Property<string>("class_id")
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

                    b.HasKey("class_id");

                    b.ToTable("Classes", (string)null);
                });

            modelBuilder.Entity("Singlearn.Models.Entities.Homework", b =>
                {
                    b.Property<int>("homework_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("homework_id"));

                    b.Property<string>("attachment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("enddate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("startdate")
                        .HasColumnType("datetime2");

                    b.Property<int>("subject_id")
                        .HasColumnType("int");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("homework_id");

                    b.ToTable("Homeworks");
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

                    b.Property<byte[]>("data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("file_type")
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

                    b.ToTable("Materials", (string)null);
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

                    b.HasIndex("stc_id");

                    b.HasIndex("template_id");

                    b.ToTable("STCTemplates", (string)null);
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

                    b.ToTable("Staff", (string)null);
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

                    b.ToTable("Subjects", (string)null);
                });

            modelBuilder.Entity("Singlearn.Models.Entities.SubjectTeacherClass", b =>
                {
                    b.Property<int>("stc_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("stc_id"));

                    b.Property<string>("class_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("subject_id")
                        .HasColumnType("int");

                    b.Property<string>("teacher_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("stc_id");

                    b.HasIndex("class_id");

                    b.HasIndex("subject_id");

                    b.HasIndex("teacher_id");

                    b.ToTable("SubjectTeacherClasses", (string)null);
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

                    b.ToTable("Templates", (string)null);
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

            modelBuilder.Entity("Singlearn.Models.Entities.STCTemplate", b =>
                {
                    b.HasOne("Singlearn.Models.Entities.SubjectTeacherClass", "SubjectTeacherClass")
                        .WithMany()
                        .HasForeignKey("stc_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Singlearn.Models.Entities.Template", "Template")
                        .WithMany()
                        .HasForeignKey("template_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubjectTeacherClass");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("Singlearn.Models.Entities.SubjectTeacherClass", b =>
                {
                    b.HasOne("Singlearn.Models.Entities.Class", "Class")
                        .WithMany()
                        .HasForeignKey("class_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Singlearn.Models.Entities.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("subject_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Singlearn.Models.Entities.Staff", "Staff")
                        .WithMany()
                        .HasForeignKey("teacher_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Staff");

                    b.Navigation("Subject");
                });
#pragma warning restore 612, 618
        }
    }
}
