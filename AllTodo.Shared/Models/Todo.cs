using AllTodo.Shared.Exceptions;
using AllTodo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AllTodo.Shared.Models
{
    public enum TodoState
    {
        NOT_STARTED,
        IN_PROGRESS,
        COMPLETED
    }


    public class TodoDTO
    {
        public int ID { get; set; } = 0;
        public int UserID { get; set; } = -1;
        public string Title { get; set; }
        public string Description { get; set; }
        public TodoState State { get; set; }

        public (bool success, string message) Validate(IUserService userservice)
        {
            return Todo.Validate(this, userservice);
        }
    }

    public class Todo
    {
        private readonly int id;
        public int ID { get { return this.id; } }

        private readonly int user_id;
        public int UserID { get { return this.user_id; } }

        // TODO: Fill in regex
        private static readonly int TITLE_MIN_LENGTH = 3;
        private static readonly int TITLE_MAX_LENGTH = 20;
        private static readonly Regex TITLE_VALIDITY_REGEX = new Regex("^*$");
        private readonly string title;
        public string Title
        {
            get { return this.title; }
        }

        // TODO: Fill in regex
        private static readonly int DESCRIPTION_MIN_LENGTH = 0;
        private static readonly int DESCRIPTION_MAX_LENGTH = 400;
        private static readonly Regex DESCRIPTION_VALIDITY_REGEX = new Regex("^*$");
        private readonly string description;
        public string Description
        {
            get { return this.description; }
        }

        private readonly TodoState state;
        public TodoState State
        {
            get { return state; }
        }


        public Todo(TodoDTO todo_dto, IUserService userservice)
        {
            this.id = todo_dto.ID;
            this.user_id = todo_dto.UserID;
            this.title = todo_dto.Title.Trim();
            this.description = todo_dto.Description.Trim();
            this.state = todo_dto.State;

            (bool success, string message) validation_result = Todo.Validate(this.user_id, this.title, this.description, this.state, userservice);

            if (validation_result.success)
                return;

            throw new InvalidInitializationException($"Creation of Todo failed. Error: {validation_result.message}");
        }

        public Todo(int id, int user_id, string title, string description, TodoState state, IUserService userservice)
        {
            this.id = id;
            this.user_id = user_id;
            this.title = title.Trim();
            this.description = description.Trim();
            this.state = state;

            (bool success, string message) validation_result = Todo.Validate(this.user_id, this.title, this.description, this.state, userservice);

            if (validation_result.success)
                return;

            throw new InvalidInitializationException($"Creation of Todo failed. Error: {validation_result.message}");
        }

        public static (bool success, string message) Validate(TodoDTO todo_dto, IUserService userservice)
        {
            return Todo.Validate(todo_dto.UserID, todo_dto.Title, todo_dto.Description, todo_dto.State, userservice);
        }

        public static (bool success, string message) Validate(int user_id, string title, string description, TodoState state, IUserService userservice)
        {
            if (!userservice.Exists(user_id))
                return (false, $"Invalid User ID: {user_id}");

            if (title == null)
                return (false, "Title cannot be null");
            if (title.Length < TITLE_MIN_LENGTH || title.Length > TITLE_MAX_LENGTH)
                return (false, $"Invalid Length of Title: {title.Length}");
            if (!TITLE_VALIDITY_REGEX.IsMatch(title))
                return (false, $"Title contained invalid characters: {title}");

            if (description == null)
                return (false, "Description cannot be null");
            if (description.Length < DESCRIPTION_MIN_LENGTH || description.Length > DESCRIPTION_MAX_LENGTH)
                return (false, $"Invalid Length of description: {description.Length}");
            if (!DESCRIPTION_VALIDITY_REGEX.IsMatch(description))
                return (false, $"Description contained invalid characters: {description}");

            if ((int)state < 0 || (int)state > 2)
                return (false, $"Invalid state: {state}");

            return (true, "Success");
        }

        public TodoDTO GetDTO()
        {
            TodoDTO dto = new TodoDTO();
            dto.ID = this.id;
            dto.UserID = this.user_id;
            dto.Title = this.title;
            dto.Description = this.description;
            dto.State = this.state;
            return dto;
        }
    }
}
