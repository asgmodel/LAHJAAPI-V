using AutoGenerator.Helper;
using AutoGenerator.Helper.Translation;
using V1.DyModels.Dso.Requests;
using V1.DyModels.Dto.Build.Requests;
using V1.Services.Services;

namespace LAHJAAPI.Utilities.Seeds;

public static class DefaultPlansAndFeatures
{

    public static async Task SeedAsync(IServiceScope scope)
    {
        var planService = scope.ServiceProvider.GetService<IUsePlanService>();
        var planFeatureService = scope.ServiceProvider.GetService<IUsePlanFeatureService>();

        //await context.PlanFeatures.ExecuteDeleteAsync();
        //await context.Plans.ExecuteDeleteAsync();
        //await context.SaveChangesAsync();
        var plan = await planService.GetOneByAsync([new FilterCondition("Id", "price_1Qn3ypKMQ7LabgRTSuyGIBVH")], new ParamOptions(["PlanFeatures"]));

        if (plan == null)
        {
            var cplans = GetPlanBuilderRepositoryList();
            //List<PlanBuilderRequestDto> plans = new List<PlanBuilderRequestDto>();
            foreach (var item in cplans)
            {
                await planService.CreateAsync(item);
                //plans.Add(plan);
            }
        }
        else if (plan?.PlanFeatures?.Count == 0 || plan?.PlanFeatures?.Count(p => p.Key == "allowed_requests") == 0)
        {
            //var cplans = GetPlanBuilderRepositoryList();
            ////List<PlanBuilderRequestDto> plans = new List<PlanBuilderRequestDto>();
            //foreach (var item in cplans)
            //{
            //    await planService.UpdateAsync(item);

            //}
        }
        //await context.Plans.AddRangeAsync(pl);
        //foreach (var plan in plans)
        //{
        //    foreach (var planFeature in plan.PlanFeatures)
        //    {
        //        planFeature.PlanId = plan.Id;
        //    }
        //    await context.PlanFeatures.AddRangeAsync(plan.PlanFeatures);
        //}a




        //await context.SaveChangesAsync();



    }

    //    private static List<CategoryCreate> GetCategories()
    //    {
    //        return new List<CategoryCreate>
    //{
    //    new CategoryCreate
    //    {
    //        //Id = "1",
    //        Name = new Dictionary<string, string>
    //        {
    //            { "ar", "تحويل النص إلى صوت" },
    //            { "en", "Text-to-Speech" }
    //        },
    //        Description = new Dictionary<string, string>
    //        {
    //            { "ar", "تحويل النصوص المكتوبة إلى صوت باستخدام تقنيات الذكاء الاصطناعي المتقدمة." },
    //            { "en", "Convert written text to speech using advanced AI technologies." }
    //        },
    //        //Active = true,
    //        //Image = "/chatbot-03.png" // يمكن تغيير صورة البطاقة هنا
    //    },
    //    new CategoryCreate
    //    {
    //        //Id = "2",
    //        Name = new Dictionary<string, string>
    //        {
    //            { "ar", "تحويل النص إلى لهجة" },
    //            { "en", "Text-to-Dialect" }
    //        },
    //        Description = new Dictionary<string, string>
    //        {
    //            { "ar", "تحويل النص إلى لهجة محددة بدقة عالية." },
    //            { "en", "Convert text into a specific dialect with high accuracy." }
    //        },
    //        //Active = true,
    //        //Image = "/chatbot-03.png" // يمكن تغيير صورة البطاقة هنا
    //    },
    //    new CategoryCreate
    //    {
    //        //Id = "3",
    //        Name = new Dictionary<string, string>
    //        {
    //            { "ar", "روبوت تفاعلي (API)" },
    //            { "en", "Interactive Bot (API)" }
    //        },
    //        Description = new Dictionary<string, string>
    //        {
    //            { "ar", "دمج روبوت تفاعلي من خلال API للعديد من المهام." },
    //            { "en", "Integrate an interactive bot through API for various tasks." }
    //        },
    //        //Active = true,
    //        //Image = "/chatbot-03.png" // يمكن تغيير صورة البطاقة هنا
    //    }
    //};
    //    }





    private static Dictionary<string, Dictionary<string, string>> Herotrial()
    {
        return new()
        {
            ["en"] = new()
            {
                ["Text1"] = "Try the power",
                ["Text2"] = "AI Accent",
                ["Description"] = "LAHJA platform provides a smart way to communicate with AI in your own dialect.",
                ["ButtonLink"] = "Start your free trial"
            },
            ["ar"] = new()
            {
                ["Text1"] = "جرب قوة",
                ["Text2"] = "لهجة AI",
                ["Description"] = "توفر  منصة لهجة  طريقة ذكية للتواصل مع الذكاء الاصطناعي بلهجتك الخاصة.",
                ["ButtonLink"] = "ابداء الاصدار التجريبي المجاني"
            }
        };

    }
    //    private static List<FAQItemCreateVM> GetFAQItems()
    //    {
    //        return new List<FAQItemCreateVM>
    //{
    //    new FAQItemCreateVM
    //    {
    //        Question = new TranslationData
    //        {
    //            Value = new Dictionary<string, string>
    //        {
    //            { "ar", "ما هو تحويل الصوت إلى نص؟" },
    //            { "en", "What is speech-to-text conversion?" }
    //        }
    //        },
    //        Answer = new TranslationData
    //        {
    //            Value = new Dictionary<string, string>
    //        {
    //            { "ar", "تحويل الصوت إلى نص هو عملية تحويل الكلام المنطوق إلى نص مكتوب باستخدام تقنيات الذكاء الاصطناعي." },
    //            { "en", "Speech-to-text conversion is the process of converting spoken language into written text using AI technologies." }
    //        }
    //        }
    //    },
    //    new FAQItemCreateVM
    //    {
    //        Question = new TranslationData
    //        {
    //            Value = new Dictionary<string, string>
    //        {
    //            { "ar", "هل يدعم نظام تحويل الصوت إلى نص اللغات واللهجات المختلفة؟" },
    //            { "en", "Does the speech-to-text system support different languages and dialects?" }
    //        },
    //        },
    //        Answer = new TranslationData
    //        {
    //            Value = new Dictionary<string, string>
    //        {
    //            { "ar", "نعم، يدعم النظام العديد من اللغات واللهجات، بما في ذلك اللهجات المحلية مثل اللهجة النجدية والحجازية." },
    //            { "en", "Yes, the system supports multiple languages and dialects, including local dialects such as Najdi and Hijazi." }
    //        }
    //        }
    //    },
    //    new FAQItemCreateVM
    //    {
    //        Question = new TranslationData
    //        {
    //            Value = new Dictionary<string, string>
    //        {
    //            { "ar", "كيف يمكنني استخدام خدمة تحويل النص إلى صوت؟" },
    //            { "en", "How can I use the text-to-speech service?" }
    //        }
    //        },
    //        Answer =new TranslationData{
    //            Value = new Dictionary<string, string>
    //        {
    //            { "ar", "يمكنك استخدام الخدمة من خلال واجهة برمجة التطبيقات (API) الخاصة بنا، حيث يمكنك إرسال النصوص واستلام الصوت الناتج." },
    //            { "en", "You can use the service through our API, where you can send texts and receive the generated audio." }
    //        }
    //        }
    //    },
    //    new FAQItemCreateVM
    //    {
    //        Question = new TranslationData
    //        {
    //            Value = new Dictionary<string, string>
    //        {
    //            { "ar", "ما هي أنواع النصوص التي يمكن تحويلها إلى صوت؟" },
    //            { "en", "What types of texts can be converted to speech?" }
    //        }
    //        },
    //        Answer = new TranslationData
    //        {
    //            Value = new Dictionary<string, string>
    //        {
    //            { "ar", "يمكن تحويل أي نص مكتوب إلى صوت، بما في ذلك النصوص العادية، المقالات، والكتب. كما يمكن تخصيص الصوت ليناسب نوع النص." },
    //            { "en", "Any written text can be converted to speech, including regular texts, articles, and books. The voice can also be customized to suit the type of text." }
    //        }
    //        }
    //    },
    //    new FAQItemCreateVM
    //    {
    //        Question = new Dictionary<string, string>
    //        {
    //            { "ar", "هل يمكن تخصيص الصوت الذي يُنتج عند تحويل النص إلى صوت؟" },
    //            { "en", "Can the voice generated in text-to-speech be customized?" }
    //        },
    //        Answer = new Dictionary<string, string>
    //        {
    //            { "ar", "نعم، يمكنك تخصيص الصوت من حيث النبرة والسرعة. يمكنك أيضًا اختيار الصوت (ذكر أو أنثى) الذي يناسب تطبيقك." },
    //            { "en", "Yes, you can customize the voice in terms of tone and speed. You can also choose the voice (male or female) that suits your application." }
    //        }
    //    },
    //    new FAQItemCreateVM
    //    {
    //        Question = new Dictionary<string, string>
    //        {
    //            { "ar", "هل يمكن تحويل النص إلى لهجة معينة؟" },
    //            { "en", "Can text be converted to a specific dialect?" }
    //        },
    //        Answer = new Dictionary<string, string>
    //        {
    //            { "ar", "نعم، نقدم خدمة تحويل النصوص إلى لهجات محلية مثل اللهجة النجدية، الحجازية، وغيرها. يمكن أيضًا تخصيص النبرة والسرعة لتناسب الاحتياجات المحلية." },
    //            { "en", "Yes, we offer text conversion services to local dialects such as the Najdi, Hijazi, and others. The tone and speed can also be tailored to meet local needs." }
    //        }
    //    },
    //    new FAQItemCreateVM
    //    {
    //        Question = new Dictionary<string, string>
    //        {
    //            { "ar", "ما هو استخدام خدمة الدردشة الفورية؟" },
    //            { "en", "What is the use of the instant chat service?" }
    //        },
    //        Answer = new Dictionary<string, string>
    //        {
    //            { "ar", "خدمة الدردشة الفورية هي نظام يعتمد على الذكاء الاصطناعي للتفاعل مع المستخدمين بشكل فوري، والإجابة على استفساراتهم. تستخدم هذه الخدمة في دعم العملاء، المساعدات الشخصية، والتفاعل مع المستخدمين في التطبيقات المختلفة." },
    //            { "en", "The instant chat service is an AI-powered system that interacts with users in real time, answering their inquiries. It is used in customer support, personal assistants, and user interaction in various applications." }
    //        }
    //    },
    //    new FAQItemCreateVM
    //    {
    //        Question = new Dictionary<string, string>
    //        {
    //            { "ar", "هل يمكن دمج هذه الخدمات مع التطبيقات الخاصة بي؟" },
    //            { "en", "Can these services be integrated with my applications?" }
    //        },
    //        Answer = new Dictionary<string, string>
    //        {
    //            { "ar", "نعم، نقدم API مفتوح للتكامل مع الأنظمة والتطبيقات الأخرى، مما يتيح لك دمج خدمات تحويل الصوت إلى نص، تحويل النص إلى صوت، تحويل النص إلى لهجة، والدردشة الفورية بسهولة." },
    //            { "en", "Yes, we provide an open API for integration with other systems and applications, allowing you to seamlessly integrate services like speech-to-text, text-to-speech, text-to-dialect conversion, and instant chat." }
    //        }
    //    },
    //    new FAQItemCreateVM
    //    {
    //        Question = new Dictionary<string, string>
    //        {
    //            { "ar", "هل يحتاج النظام إلى اتصال بالإنترنت؟" },
    //            { "en", "Does the system require an internet connection?" }
    //        },
    //        Answer = new Dictionary<string, string>
    //        {
    //            { "ar", "نعم، بعض الخدمات قد تتطلب اتصالاً بالإنترنت، خاصة عندما يتم معالجة البيانات على الخوادم. ولكن هناك أيضًا خيارات لتشغيل بعض الخدمات محليًا في بيئات لا تدعم الاتصال المستمر بالإنترنت." },
    //            { "en", "Yes, some services may require an internet connection, especially when processing data on servers. However, there are also options for running certain services locally in environments that do not support continuous internet connectivity." }
    //        }
    //    },
    //    new FAQItemCreateVM
    //    {
    //        Question = new Dictionary<string, string>
    //        {
    //            { "ar", "كيف يمكنني تخصيص الخدمة لتناسب احتياجاتي؟" },
    //            { "en", "How can I customize the service to meet my needs?" }
    //        },
    //        Answer = new Dictionary<string, string>
    //        {
    //            { "ar", "يمكنك تخصيص الخدمة من خلال تحديد إعدادات الصوت، النبرة، السرعة، اللهجات، بالإضافة إلى تكامل الأنظمة عبر API. إذا كانت لديك متطلبات خاصة، يمكننا العمل معك لتوفير حل مخصص يناسب احتياجاتك." },
    //            { "en", "You can customize the service by configuring voice settings, tone, speed, and dialects, as well as system integration via API. If you have specific requirements, we can work with you to provide a tailored solution." }
    //        }
    //    },
    //    new FAQItemCreateVM
    //    {
    //        Question = new Dictionary<string, string>
    //        {
    //            { "ar", "هل يمكنني استخدام الخدمة في التطبيقات متعددة المنصات؟" },
    //            { "en", "Can I use the service on multiple platforms?" }
    //        },
    //        Answer = new Dictionary<string, string>
    //        {
    //            { "ar", "نعم، يمكن استخدام هذه الخدمات عبر مختلف المنصات مثل الهواتف المحمولة (Android و iOS)، وأجهزة الكمبيوتر، ويمكن أيضًا دمجها في تطبيقات الويب والتطبيقات التي تعمل في الخلفية." },
    //            { "en", "Yes, these services can be used across various platforms such as mobile phones (Android and iOS), computers, and can also be integrated into web applications and background processes." }
    //        }
    //    },
    //    new FAQItemCreateVM
    //    {
    //        Question = new Dictionary<string, string>
    //        {
    //            { "ar", "ما هي تكلفة استخدام هذه الخدمات؟" },
    //            { "en", "What is the cost of using these services?" }
    //        },
    //        Answer = new Dictionary<string, string>
    //        {
    //            { "ar", "تعتمد تكلفة الخدمة على الخطة التي تختارها وعدد الطلبات التي تحتاج إليها. يمكنك التواصل معنا للحصول على تفاصيل حول الأسعار وفقًا لاحتياجاتك." },
    //            { "en", "The cost of the service depends on the plan you choose and the number of requests you need. You can contact us for details about pricing based on your requirements." }
    //        }
    //    }
    //};
    //    }


    static List<PlanFeatureRequestBuildDto> GetPlanFeatureCreateFree()
    {
        var planFeatures = new List<PlanFeatureRequestBuildDto>
                  {
                        new ()
                        {
                            Key = "number_models",
                            Value = "3",
                            Name = new TranslationData
                            {
                                Value = new Dictionary<string, string>
                                {
                                    { "en", "AI Models" },
                                    { "ar", "عدد النماذج AI" }
                                }
                            },
                            Description = new TranslationData{
                                Value = new Dictionary<string, string>
                                {
                                    { "en", "3" },
                                    { "ar", "3" }
                                }
                            }
                        },
                        new ()
                        {
                            Key = "allowed_requests",
                            Value = "1000",
                            Name = new TranslationData
                            {
                                Value = new Dictionary<string, string>
                            {
                                { "en", "Requests" },
                                { "ar", "الطلبات" }
                            }
                            },
                            Description = new TranslationData
                            {
                                Value = new Dictionary<string, string>
                            {
                                { "en", "1,000 request" },
                                { "ar", "1,000 طلب" }
                            }
                            }
                        },
                        new ()
                            {
                                Key = "processor",
                                Value = "shared",
                                Name = new TranslationData
                                {
                                    Value = new Dictionary<string, string>
                                    {
                                        { "en", "Processor" },
                                        { "ar", "المعالج" }
                                    }
                                },
                                Description = new TranslationData
                                {
                                    Value = new Dictionary<string, string>
                                    {
                                        { "en", "Shared" },
                                        { "ar", "مشترك" }
                                    }
                                }
                            },
                        new ()
{
    Key = "ram",  // Changed from Id to Key
    Value = "2",
    Name = new TranslationData
    {
        Value = new Dictionary<string, string>
        {
            ["en"] = "RAM",
            ["ar"] = "الذاكرة العشوائية"
        }
    },
    Description = new TranslationData
    {
        Value = new Dictionary<string, string>
        {
            ["en"] = "CPU 2 GB",
            ["ar"] = "CPU 2 جيجابايت"
        }
    }
},

    new()
    {
        Key = "speed",
        Value = "2",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Speed",
                ["ar"] = "السرعة"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "2 pre/Second",
                ["ar"] = "2 pre/Second"
            }
        }
    },
    new()
    {
        Key = "support",
        Value = "no",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Support",
                ["ar"] = "الدعم"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "No",
                ["ar"] = "لا"
            }
        }
    },
    new()
    {
        Key = "customization",
        Value = "no",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Customization",
                ["ar"] = "تخصيص"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "No",
                ["ar"] = "لا"
            }
        }
    },
    new()
    {
        Key = "api",
        Value = "no",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "API",
                ["ar"] = "API"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "No",
                ["ar"] = "لا"
            }
        }
    },
    new()
    {
        Key = "allowed_spaces",
        Value = "1",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Space",
                ["ar"] = "Space"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "1",
                ["ar"] = "1"
            }
        }
    }

        };

        return planFeatures;

    }



    static List<PlanFeatureRequestBuildDto> GetPlanFeatureCreateStandard()
    {
        var planFeatures = new List<PlanFeatureRequestBuildDto>
{
    new()
    {
        Key = "number_models",
        Value = "3",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "AI Models",
                ["ar"] = "عدد النماذج AI"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "3",
                ["ar"] = "3"
            }
        }
    },
    new()
    {
        Key = "allowed_requests",
        Value = "10000",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Requests",
                ["ar"] = "الطلبات"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "10,000",
                ["ar"] = "10,000"
            }
        }
    },
    new()
    {
        Key = "processor",
        Value = "2",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Processor",
                ["ar"] = "المعالج"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "CPU 2 GB",
                ["ar"] = "CPU 2 جيجابايت"
            }
        }
    },
    new()
    {
        Key = "ram",
        Value = "2",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "RAM",
                ["ar"] = "الذاكرة العشوائية"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "CPU 2 GB",
                ["ar"] = "CPU 2 جيجابايت"
            }
        }
    },
    new()
    {
        Key = "speed",
        Value = "1",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Speed",
                ["ar"] = "السرعة"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "1 per/Second",
                ["ar"] = "1 في الثانية"
            }
        }
    },
    new()
    {
        Key = "support",
        Value = "no",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Support",
                ["ar"] = "الدعم"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "No",
                ["ar"] = "لا"
            }
        }
    },
    new()
    {
        Key = "customization",
        Value = "no",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Customization",
                ["ar"] = "تخصيص"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "No",
                ["ar"] = "لا"
            }
        }
    },
    new()
    {
        Key = "api",
        Value = "yes",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "API",
                ["ar"] = "API"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Yes",
                ["ar"] = "نعم"
            }
        }
    },
    new()
    {
        Key = "allowed_spaces",
        Value = "3",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Space",
                ["ar"] = "Space"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "3",
                ["ar"] = "3"
            }
        }
    },
    new()
    {
        Key = "scalability",
        Value = "Twice amonth",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Scalability",
                ["ar"] = "قابلية التوسيع"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Twice amonth",
                ["ar"] = "مرتين شهريا"
            }
        }
    }
};
        return planFeatures;

    }



    static List<PlanFeatureRequestBuildDto> GetPlanFeatureCreateProfessional()
    {
        var planFeatures = new List<PlanFeatureRequestBuildDto>
{
    new()
    {
        Key = "number_models",
        Value = "12",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "AI Models",
                ["ar"] = "عدد النماذج AI"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "12",
                ["ar"] = "12"
            }
        }
    },
    new()
    {
        Key = "allowed_requests",
        Value = "100000",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Requests",
                ["ar"] = "الطلبات"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "100,000",
                ["ar"] = "100,000"
            }
        }
    },
    new()
    {
        Key = "processor",
        Value = "4",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Processor",
                ["ar"] = "المعالج"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "CPU 4 GB",
                ["ar"] = "CPU 4 جيجابايت"
            }
        }
    },
    new()
    {
        Key = "ram",
        Value = "8",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "RAM",
                ["ar"] = "الذاكرة العشوائية"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "CPU 8 GB",
                ["ar"] = "CPU 8 جيجابايت"
            }
        }
    },
    new()
    {
        Key = "speed",
        Value = "0.5",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Speed",
                ["ar"] = "السرعة"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "0.5 pre/Second",
                ["ar"] = "0.5 في الثانية"
            }
        }
    },
    new()
    {
        Key = "support",
        Value = "yes",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Support",
                ["ar"] = "الدعم"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Yes",  // Fixed typo from "Yse"
                ["ar"] = "نعم"
            }
        }
    },
    new()
    {
        Key = "customization",
        Value = "yes",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Customization",
                ["ar"] = "تخصيص"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Yes",  // Fixed typo from "Yse"
                ["ar"] = "نعم"
            }
        }
    },
    new()
    {
        Key = "api",
        Value = "yes",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "API",
                ["ar"] = "API"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Yes",
                ["ar"] = "نعم"
            }
        }
    },
    new()
    {
        Key = "allowed_spaces",
        Value = "10",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Space",
                ["ar"] = "المساحات المسموح بها"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "10",
                ["ar"] = "10"
            }
        }
    },
    new()
    {
        Key = "scalability",
        Value = "Unlimited",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Scalability",
                ["ar"] = "قابلية التوسيع"  // Removed extra colon
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Unlimited",
                ["ar"] = "غير محدد"
            }
        }
    }
};



        return planFeatures;

    }

    static List<PlanFeatureRequestBuildDto> GetPlanFeatureCreateEnterprise()
    {



        var planFeatures = new List<PlanFeatureRequestBuildDto>
{
    new()
    {
        Key = "number_models",
        Value = "12",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "AI Models",
                ["ar"] = "عدد النماذج AI"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "12",
                ["ar"] = "12"
            }
        }
    },
    new()
    {
        Key = "allowed_requests",
        Value = "Unlimited",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Requests",
                ["ar"] = "الطلبات"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Unlimited",
                ["ar"] = "غير محدد"
            }
        }
    },
    new()
    {
        Key = "processor",
        Value = "Unlimited",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Processor",
                ["ar"] = "المعالج"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Unlimited",
                ["ar"] = "غير محدد"
            }
        }
    },
    new()
    {
        Key = "ram",
        Value = "Unlimited",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "RAM",
                ["ar"] = "الذاكرة العشوائية"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Unlimited",
                ["ar"] = "غير محدد"
            }
        }
    },
    new()
    {
        Key = "speed",
        Value = "0.5",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Speed",
                ["ar"] = "السرعة"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "0.5 pre/Second",
                ["ar"] = "0.5 في الثانية"
            }
        }
    },
    new()
    {
        Key = "support",
        Value = "yes",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Support",
                ["ar"] = "الدعم"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Yes",  // Fixed typo from "Yse"
                ["ar"] = "نعم"
            }
        }
    },
    new()
    {
        Key = "customization",
        Value = "yes",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Customization",
                ["ar"] = "تخصيص"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Yes",  // Fixed typo from "Yse"
                ["ar"] = "نعم"
            }
        }
    },
    new()
    {
        Key = "api",
        Value = "yes",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "API",
                ["ar"] = "API"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Yes",
                ["ar"] = "نعم"
            }
        }
    },
    new()
    {
        Key = "allowed_spaces",
        Value = "unlimited",
        Name = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Space",
                ["ar"] = "Space"
            }
        },
        Description = new TranslationData
        {
            Value = new Dictionary<string, string>
            {
                ["en"] = "Unlimited",
                ["ar"] = "غير محدد"
            }
        }
    }
};



        return planFeatures;

    }

    public static List<PlanRequestDso> GetPlanBuilderRepositoryList()
    {
        var cplans = new List<PlanRequestDso>()
        {

           new PlanRequestDso()
           {
                Id = "price_1Qn3ypKMQ7LabgRTSuyGIBVH",

                ProductId = "prod_RgQqgxzSIFATNu",
               ProductName=new TranslationData
               {
                   Value = new Dictionary<string, string>()
               {
                                 { "en", "Free" },
                                { "ar", "Free" }
               }
               }

              ,
               Description=new TranslationData
               {
                   Value = new Dictionary<string, string>()
               {
                   // حط الوصف 
                      { "en", "Free" },
                    { "ar", "خطة اشتراك أساسية" }
               }
               },
               Amount=0,
               BillingPeriod="month",
               PlanFeatures=GetPlanFeatureCreateFree()
           }
           ,
        new PlanRequestDso()
{
    Id = "price_1Qn3yrKMQ7LabgRT4wwrFO8N",
    ProductId = "prod_RgQq5zSmM2Xvtm",
    ProductName = new TranslationData
    {
        Value = new Dictionary<string, string>
        {
            ["en"] = "Standard",
            ["ar"] = "Standard"
        }
    },
    Description = new TranslationData
    {
        Value = new Dictionary<string, string>
        {
            ["en"] = "Intermediate subscription plan",
            ["ar"] = "خطة اشتراك متوسطة"
        }
    },
    BillingPeriod = "month",
    Amount = 150,
    PlanFeatures = GetPlanFeatureCreateStandard()
},
     new PlanRequestDso()
{
    Id = "price_1Qn3ysKMQ7LabgRTxBd8TqEQ",
    ProductId = "prod_RgQqo1YRO6pPxL",
    ProductName = new TranslationData
    {
        Value = new Dictionary<string, string>
        {
            ["en"] = "Professional",
            ["ar"] = "Professional"
        }
    },
    Description = new TranslationData
    {
        Value = new Dictionary<string, string>
        {
            ["en"] = "Professional subscription plan",
            ["ar"] = "خطة اشتراك احترافية"
        }
    },
    BillingPeriod = "month",
    Amount = 250.0,
    PlanFeatures = GetPlanFeatureCreateProfessional()
},

new PlanRequestDso()
{
    Id = "price_1Qn3ysKMQ7LabgRT1KU7AcSL",
    ProductId = "prod_RgQq2S3XeSvAQS",
    ProductName = new TranslationData
    {
        Value = new Dictionary<string, string>
        {
            ["en"] = "Enterprise",
            ["ar"] = "Enterprise"
        }
    },
    Description = new TranslationData
    {
        Value = new Dictionary<string, string>
        {
            ["en"] = "Advanced subscription plan for enterprises",
            ["ar"] = "خطة اشتراك متقدمة للمؤسسات"
        }
    },
    BillingPeriod = "month",
    Amount = 1000,
    PlanFeatures = GetPlanFeatureCreateEnterprise()
}


        };
        // ضيف بقية الخطط والمعلومات 


        //var plans = mapper.Map<List<Plan>>(cplans);

        return cplans;
    }

    //private static Service[] GetServices(string modelAiId)
    //{
    //    Service[] services = [
    //    new Service() { Name = "Text to text", Token = "bearer",AbsolutePath="t2t" ,ModelAiId=modelAiId},
    //            new Service() { Name = "Text To Speech",AbsolutePath="t2speech", Token = "bearer",ModelAiId=modelAiId },
    //            new Service() { Name = "VoiceBot",AbsolutePath = "speaker", Token = "bearer" , ModelAiId = modelAiId},
    //            ];

    //    return services;
    //}

    //private static List<Plan> GetPlans()
    //{

    //    List<Plan> plans =
    //    [
    //         new()
    //        {
    //            Id = "price_1QSOh8KMQ7LabgRTu8QHKFJE",
    //            BillingPeriod = "month",
    //            //NumberRequests = 10,
    //            ProductId = "prod_RL4cPSzDwjdQyh",
    //            ProductName = "Free",
    //            Description="DDDFGFGF",
    //            Amount = 0,
    //            Images=null,
    //            Active=true,
    //            UpdatedAt=DateTime.Today,
    //            CreatedAt=DateTime.Today



    ////public required long NumberRequests { get; set; }


    //        }


    //    ];

    //    return plans;

    //}

}