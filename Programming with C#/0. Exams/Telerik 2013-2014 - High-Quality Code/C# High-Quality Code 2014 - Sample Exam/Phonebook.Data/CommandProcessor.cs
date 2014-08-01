﻿namespace Phonebook.Data
{
    using System;
    using System.Collections.Generic;
    using Phonebook.Data.Commands;
    using Phonebook.Data.Contracts;
    using Phonebook.Models.Contracts;

    //!++ This class in unnecessary
    //! Command patterns
    public class CommandProcessor : ICommandProcessor
    {
        private const string AddPhoneCommandName = "AddPhone";
        private const string DeletePhoneCommandName = "DeletePhone";
        private const string ChangePhoneCommandName = "ChangePhone";
        private const string ListPhonesCommandName = "List";

        private readonly IDictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        //! Dependency inversion
        public CommandProcessor()
            : this(new PhonebookRepositoryFast())
        {
        }

        public CommandProcessor(IPhonebookRepository phonebookRepository)
        {
            this.InitializeCommandList(phonebookRepository);
        }
 
        public string ProcessCommand(string @string, ICommandParser commandParser)
        {
            if (string.IsNullOrEmpty(@string))
            {
                throw new ArgumentException("Input string cannot be null or empty.");
            }

            var command = commandParser.Parse(@string);
            return this.ProcessCommand(command);
        }

        public string ProcessCommand(IPhonebookCommand command)
        {
            if (command == null)
            {
                throw new NullReferenceException("Input command instance cannot be null.");
            }

            ICommand selectedCommand;
            this.commands.TryGetValue(command.Name, out selectedCommand);
            if (selectedCommand == null)
            {
                throw new ArgumentException("You have entered unknown command name.");
            }
            
            return selectedCommand.Execute(command.Arguments);
        }

        private void InitializeCommandList(IPhonebookRepository phonebookRepository)
        {
            if (phonebookRepository == null)
            {
                throw new NullReferenceException("Phonebook repository instance cannot be null.");
            }

            this.commands[AddPhoneCommandName] = new AddPhoneCommand(phonebookRepository);
            this.commands[DeletePhoneCommandName] = new DeletePhoneCommand(phonebookRepository);
            this.commands[ChangePhoneCommandName] = new ChangePhoneCommand(phonebookRepository);
            this.commands[ListPhonesCommandName] = new ListPhonesCommand(phonebookRepository);
        }
    }
}