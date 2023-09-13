using QuestionnaireService.Models;

namespace QuestionnaireService.DbContexts;

public static class DbInitializer
{
    public static void Initialize(QuestionnaireDbContext context)
    {
        context.Database.EnsureCreated();
        
        if (context.Questionnaires.Any()) return; // DB has been seeded

        SeedData(context);
    }
    
    private static void SeedData(QuestionnaireDbContext context)
    {
        var questionnaire = new Questionnaire
        {
            Subject = "Cyber Quant"
        };

        context.Questionnaires.Add(questionnaire);
        
        SeedQuestion1Data(context, questionnaire);
        SeedQuestion2Data(context, questionnaire);
        SeedQuestion3Data(context, questionnaire);
        SeedQuestion4Data(context, questionnaire);
        SeedQuestion5Data(context, questionnaire);
        
        SeedRespondent1Data(context);
        SeedRespondent2Data(context);
    }

    private static void SeedRespondent2Data(QuestionnaireDbContext context)
    {
        var respondent2 = new Respondent
        {
            Name = "Eve"
        };

        context.Respondents.Add(respondent2);

        context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent2,
            Question = context.Questions.ToArray()[0],
            Answer = context.Questions.ToArray()[0].Answers.ToList()[1]
        });

        context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent2,
            Question = context.Questions.ToArray()[0],
            Answer = context.Questions.ToArray()[0].Answers.ToList()[2]
        });

        context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent2,
            Question = context.Questions.ToArray()[1],
            Answer = context.Questions.ToArray()[1].Answers.ToList()[2]
        });
        
        context.SaveChanges();
    }

    private static void SeedRespondent1Data(QuestionnaireDbContext context)
    {
        var respondent1 = new Respondent
        {
            Name = "Eido"
        };

        context.Respondents.Add(respondent1);

        context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent1,
            Question = context.Questions.ToArray()[0],
            Answer = context.Questions.ToArray()[0].Answers!.ToList()[0]
        });

        context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent1,
            Question = context.Questions.ToArray()[1],
            Answer = context.Questions.ToArray()[1].Answers.ToList()[0]
        });
        
        context.SaveChanges();
    }

    private static void SeedQuestion1Data(QuestionnaireDbContext context, Questionnaire questionnaire)
    {
        var question = new Question
        {
            Text = "What does Cyber Quant aim to measure in an organization's cybersecurity?",
            IsSingleChoice = false, 
            Comment = "That is a tricky question",
            Questionnaire = questionnaire
        };
        
        context.Questions.Add(question);
        
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "Impact of new cybersecurity controls",
            Score = 95
        });
            
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "Financial risk of security breaches",
            Score = 25
        });
            
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "How many coffee cups the IT team consumes",
            Score = 10
        });
        
        context.SaveChanges();
    }
    
    private static void SeedQuestion2Data(QuestionnaireDbContext context, Questionnaire questionnaire)
    {
        var question = new Question
        {
            Text = "In the case study, how much did the bank decrease their financial risk by using Cyber Quant?",
            IsSingleChoice = true,
            Questionnaire = questionnaire
        };
        
        context.Questions.Add(question);
        
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "$5",
            Score = 0
        });
            
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "$100 million",
            Score = 80
        });
            
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "$155 million",
            Score = 90
        });
        
        context.SaveChanges();
    }
    
    private static void SeedQuestion3Data(QuestionnaireDbContext context, Questionnaire questionnaire)
    {
        var question = new Question
        {
            Text = "What is the goal of Cyber Quant's good user validation solution?",
            IsSingleChoice = false,
            Comment = "You know the answer",
            Questionnaire = questionnaire
        };
        
        context.Questions.Add(question);
        
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "Protecting online environments from account hacking",
            Score = 90
        });
            
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "Assessing third-party risks",
            Score = 10
        });
            
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "Prioritizing cybersecurity investments",
            Score = 25
        });
        
        context.SaveChanges();
    }
    
    private static void SeedQuestion4Data(QuestionnaireDbContext context, Questionnaire questionnaire)
    {
        var question = new Question
        {
            Text = "How long does a typical Cyber Quant consulting engagement last?",
            IsSingleChoice = true,
            Questionnaire = questionnaire
        };
        
        context.Questions.Add(question);
        
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "1 hour",
            Score = 0
        });
            
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "1 week",
            Score = 30
        });

        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "3 weeks",
            Score = 80
        });
        
        context.SaveChanges();
    }
    
    private static void SeedQuestion5Data(QuestionnaireDbContext context, Questionnaire questionnaire)
    {
        var question = new Question
        {
            Text = "How many cybersecurity controls does Cyber Quant assess?",
            IsSingleChoice = false,
            Comment = "You can do it!",
            Questionnaire = questionnaire
        };
        
        context.Questions.Add(question);
        
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "70",
            Score = 30
        });
            
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "50",
            Score = 100
        });
            
        context.Answers.Add(new Answer
        {
            Question = question,
            Text = "It depends whether the customer is liken by us",
            Score = 5
        });
        
        context.SaveChanges();
    }
}