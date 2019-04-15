using AllTodo.Shared.Exceptions;
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

    public class Todo
    {
        public int id { get; private set; }

        // TODO: Fill these in
        private static readonly int TITLE_MIN_LENGTH = 0;
        private static readonly int TITLE_MAX_LENGTH = 0;
        private static readonly Regex TITLE_VALIDITY_REGEX = new Regex("");
        private readonly string title;
        public string Title
        {
            get { return this.title; }
        }

        // TODO: Fill these in
        private static readonly int DESCRIPTION_MIN_LENGTH = 0;
        private static readonly int DESCRIPTION_MAX_LENGTH = 0;
        private static readonly Regex DESCRIPTION_VALIDITY_REGEX = new Regex("");
        private readonly string description;
        public string Description
        {
            get { return this.description; }
        }

        public Todo(int id, string title, string description, TodoState state = TodoState.NOT_STARTED)
        {
            this.id = id;

            this.title = title.Trim();
            if (this.title.Length < TITLE_MIN_LENGTH || this.title.Length > TITLE_MAX_LENGTH)
                throw new InvalidInitializationException("Invalid Length of Title");
            if (!TITLE_VALIDITY_REGEX.IsMatch(this.title))
                throw new InvalidInitializationException("Title contained invalid characters");

            this.description = description.Trim();
            if (this.description.Length < DESCRIPTION_MIN_LENGTH || this.description.Length > DESCRIPTION_MAX_LENGTH)
                throw new InvalidInitializationException("Invalid Length of Description");
            if (!DESCRIPTION_VALIDITY_REGEX.IsMatch(this.description))
                throw new InvalidInitializationException("Description contained invalid characters");
        }

        
    }
}
