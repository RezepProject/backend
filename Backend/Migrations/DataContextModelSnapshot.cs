﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using backend;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("QuestionQuestionCategory", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("integer")
                        .HasColumnName("categories_id");

                    b.Property<int>("QuestionsId")
                        .HasColumnType("integer")
                        .HasColumnName("questions_id");

                    b.HasKey("CategoriesId", "QuestionsId")
                        .HasName("pk_questionquestioncategory_dictionary_string_object");

                    b.HasIndex("QuestionsId")
                        .HasDatabaseName("ix_questionquestioncategory_dictionary_string_object_quest");

                    b.ToTable("questionquestioncategory (dictionary<string, object>)", (string)null);
                });

            modelBuilder.Entity("backend.Entities.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("QuestionId")
                        .HasColumnType("integer")
                        .HasColumnName("question_id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user");

                    b.HasKey("Id")
                        .HasName("pk_answer");

                    b.HasIndex("QuestionId")
                        .HasDatabaseName("ix_answer_question_id");

                    b.ToTable("answer", (string)null);
                });

            modelBuilder.Entity("backend.Entities.Config", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_config");

                    b.ToTable("config", (string)null);
                });

            modelBuilder.Entity("backend.Entities.ConfigUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("refresh_token");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer")
                        .HasColumnName("role_id");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("token_created");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("token_expires");

                    b.HasKey("Id")
                        .HasName("pk_configuser");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_configuser_role_id");

                    b.ToTable("configuser", (string)null);
                });

            modelBuilder.Entity("backend.Entities.ConfigUserToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer")
                        .HasColumnName("role_id");

                    b.Property<Guid>("Token")
                        .HasColumnType("uuid")
                        .HasColumnName("token");

                    b.HasKey("Id")
                        .HasName("pk_configusertoken");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_configusertoken_role_id");

                    b.ToTable("configusertoken", (string)null);
                });

            modelBuilder.Entity("backend.Entities.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.HasKey("Id")
                        .HasName("pk_question");

                    b.ToTable("question", (string)null);
                });

            modelBuilder.Entity("backend.Entities.QuestionCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_questioncategory");

                    b.ToTable("questioncategory", (string)null);
                });

            modelBuilder.Entity("backend.Entities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.HasKey("Id")
                        .HasName("pk_refreshtoken");

                    b.ToTable("refreshtoken", (string)null);
                });

            modelBuilder.Entity("backend.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_role");

                    b.ToTable("role", (string)null);
                });

            modelBuilder.Entity("backend.Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AiInUse")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ai_in_use");

                    b.Property<string>("BackgroundImage")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("background_image");

                    b.Property<int>("ConfigUser")
                        .HasColumnType("integer")
                        .HasColumnName("config_user");

                    b.Property<int>("ConfigUserId")
                        .HasColumnType("integer")
                        .HasColumnName("config_user_id");

                    b.Property<string>("GreetingMessage")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("greeting_message");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("language");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<bool>("State")
                        .HasColumnType("boolean")
                        .HasColumnName("state");

                    b.Property<double>("TalkingSpeed")
                        .HasColumnType("double precision")
                        .HasColumnName("talking_speed");

                    b.HasKey("Id")
                        .HasName("pk_setting");

                    b.ToTable("setting", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AiInUse = "ChatGPT",
                            BackgroundImage = "https://example.com/image.jpg",
                            ConfigUser = 0,
                            ConfigUserId = 0,
                            GreetingMessage = "Hello, how can I help you?",
                            Language = "en-US",
                            Name = "Rezep-1",
                            State = true,
                            TalkingSpeed = 0.69999999999999996
                        });
                });

            modelBuilder.Entity("QuestionQuestionCategory", b =>
                {
                    b.HasOne("backend.Entities.QuestionCategory", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_questionquestioncategory_dictionary_string_object_quest");

                    b.HasOne("backend.Entities.Question", null)
                        .WithMany()
                        .HasForeignKey("QuestionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_questionquestioncategory_dictionary_string_object_quest1");
                });

            modelBuilder.Entity("backend.Entities.Answer", b =>
                {
                    b.HasOne("backend.Entities.Question", null)
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .HasConstraintName("fk_answer_questions_question_id");
                });

            modelBuilder.Entity("backend.Entities.ConfigUser", b =>
                {
                    b.HasOne("backend.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_configuser_roles_role_id");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("backend.Entities.ConfigUserToken", b =>
                {
                    b.HasOne("backend.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_configusertoken_roles_role_id");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("backend.Entities.Question", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("backend.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
