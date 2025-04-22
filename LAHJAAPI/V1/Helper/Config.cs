using Quartz;
using ApiCore.Validators;
using AutoGenerator.Schedulers;
using System.Reflection;
using System;

namespace ApiCore.Helper
{
    public static class TemplateTagEmail 
    {
             public static string SubscriptionExpiringSoonTemplate(string userName, DateTime expiryDate)
        {
            string formattedDate = expiryDate.ToString("yyyy/MM/dd");
            string template = @"
<!DOCTYPE html>
<html lang='ar'>
<head>
    <meta charset='UTF-8'>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background-color: #f2f4f6; direction: rtl; }
        .card { max-width: 600px; margin: 40px auto; background: #fff; border-radius: 12px; padding: 30px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); }
        .header { font-size: 24px; color: #ffc107; margin-bottom: 10px; display: flex; align-items: center; }
        .icon { font-size: 28px; margin-left: 10px; }
        .message { font-size: 16px; color: #555; line-height: 1.6; }
        .footer { font-size: 12px; color: #999; margin-top: 30px; }
    </style>
</head>
<body>
    <div class='card'>
        <div class='header'><span class='icon'>⏳</span>تنبيه بانتهاء الاشتراك</div>
        <div class='message'>
            عزيزي {{user_name}}،<br/>
            نود إعلامك بأن اشتراكك سينتهي بتاريخ <strong>{{expiry_date}}</strong>.<br/>
            يرجى التجديد قريبًا لضمان استمرارية الخدمة دون انقطاع.
        </div>
        <div class='footer'>
            إذا قمت بالفعل بتجديد الاشتراك، يرجى تجاهل هذه الرسالة.
        </div>
    </div>
</body>
</html>";
            return template.Replace("{{user_name}}", userName).Replace("{{expiry_date}}", formattedDate);
        }

    
        public static string GetConfirmationEmailHtml(string confirmationLink)
        {
            string emailTemplate = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>تأكيد البريد الإلكتروني</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            direction: rtl;
            background-color: #f5f5f5;
            padding: 20px;
        }
        .container {
            background-color: #ffffff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px #cccccc;
        }
        .btn {
            display: inline-block;
            padding: 12px 20px;
            background-color: #28a745;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 20px;
        }
        .footer {
            margin-top: 30px;
            font-size: 12px;
            color: #777777;
        }
    </style>
</head>
<body>
    <div class='container'>
        <h2>مرحباً بك!</h2>
        <p>شكراً لتسجيلك. لتأكيد بريدك الإلكتروني، يرجى الضغط على الزر أدناه:</p>
        <a class='btn' href='{{confirmation_link}}'>تأكيد البريد الإلكتروني</a>
        <p class='footer'>إذا لم تقم بإنشاء هذا الحساب، يمكنك تجاهل هذه الرسالة.</p>
    </div>
</body>
</html>";

            return emailTemplate.Replace("{{confirmation_link}}", confirmationLink);
        }




        public static string EmailConfirmationTemplate(string confirmationLink)
        {
            string template = @"
<!DOCTYPE html>
<html lang='ar'>
<head>
    <meta charset='UTF-8'>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background-color: #f2f4f6; margin: 0; padding: 0; direction: rtl; }
        .card { max-width: 600px; margin: 40px auto; background: #fff; border-radius: 12px; padding: 30px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); }
        .header { font-size: 24px; color: #333; margin-bottom: 10px; display: flex; align-items: center; }
        .icon { font-size: 28px; margin-left: 10px; }
        .message { font-size: 16px; color: #555; line-height: 1.6; }
        .btn { display: inline-block; margin-top: 20px; padding: 12px 24px; background: #28a745; color: #fff; border-radius: 6px; text-decoration: none; font-size: 16px; }
        .footer { font-size: 12px; color: #999; margin-top: 30px; }
    </style>
</head>
<body>
    <div class='card'>
        <div class='header'><span class='icon'>✅</span>تأكيد البريد الإلكتروني</div>
        <div class='message'>
            شكرًا لتسجيلك لدينا! الرجاء تأكيد بريدك الإلكتروني بالضغط على الزر أدناه:
        </div>
        <a class='btn' href='{{confirmation_link}}'>تأكيد الآن</a>
        <div class='footer'>إذا لم تقم بإنشاء هذا الحساب، يمكنك تجاهل هذه الرسالة.</div>
    </div>
</body>
</html>";
            return template.Replace("{{confirmation_link}}", confirmationLink);
        }



        public static string PasswordResetTemplate(string resetLink)
        {
            string template = @"
<!DOCTYPE html>
<html lang='ar'>
<head>
    <meta charset='UTF-8'>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background-color: #f2f4f6; direction: rtl; }
        .card { max-width: 600px; margin: 40px auto; background: #fff; border-radius: 12px; padding: 30px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); }
        .header { font-size: 24px; color: #333; margin-bottom: 10px; display: flex; align-items: center; }
        .icon { font-size: 28px; margin-left: 10px; }
        .message { font-size: 16px; color: #555; line-height: 1.6; }
        .btn { display: inline-block; margin-top: 20px; padding: 12px 24px; background: #007bff; color: #fff; border-radius: 6px; text-decoration: none; font-size: 16px; }
        .footer { font-size: 12px; color: #999; margin-top: 30px; }
    </style>
</head>
<body>
    <div class='card'>
        <div class='header'><span class='icon'>🔒</span>إعادة تعيين كلمة المرور</div>
        <div class='message'>
            لقد طلبت إعادة تعيين كلمة المرور. اضغط على الزر أدناه لإتمام العملية:
        </div>
        <a class='btn' href='{{reset_link}}'>إعادة التعيين</a>
        <div class='footer'>إذا لم تطلب هذه العملية، يرجى تجاهل هذه الرسالة.</div>
    </div>
</body>
</html>";
            return template.Replace("{{reset_link}}", resetLink);
        }



        public static string WelcomeEmailTemplate(string userName)
        {
            string template = @"
<!DOCTYPE html>
<html lang='ar'>
<head>
    <meta charset='UTF-8'>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background-color: #f2f4f6; direction: rtl; }
        .card { max-width: 600px; margin: 40px auto; background: #fff; border-radius: 12px; padding: 30px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); }
        .header { font-size: 24px; color: #333; margin-bottom: 10px; display: flex; align-items: center; }
        .icon { font-size: 28px; margin-left: 10px; }
        .message { font-size: 16px; color: #555; line-height: 1.6; }
    </style>
</head>
<body>
    <div class='card'>
        <div class='header'><span class='icon'>🎉</span>مرحباً {{user_name}}!</div>
        <div class='message'>
            يسعدنا انضمامك إلينا! نتمنى لك تجربة رائعة وممتعة في منصتنا.
        </div>
    </div>
</body>
</html>";
            return template.Replace("{{user_name}}", userName);
        }
        public static string SecurityAlertTemplate(string location, string time)
        {
            string template = @"
<!DOCTYPE html>
<html lang='ar'>
<head>
    <meta charset='UTF-8'>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background-color: #f2f4f6; direction: rtl; }
        .card { max-width: 600px; margin: 40px auto; background: #fff; border-radius: 12px; padding: 30px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); }
        .header { font-size: 24px; color: #d9534f; margin-bottom: 10px; display: flex; align-items: center; }
        .icon { font-size: 28px; margin-left: 10px; }
        .message { font-size: 16px; color: #555; line-height: 1.6; }
    </style>
</head>
<body>
    <div class='card'>
        <div class='header'><span class='icon'>⚠️</span>تنبيه أمني</div>
        <div class='message'>
            تم تسجيل دخول جديد إلى حسابك من الموقع: <strong>{{location}}</strong> في الوقت: <strong>{{time}}</strong>.
            إذا لم تكن أنت، يرجى تغيير كلمة المرور فورًا.
        </div>
    </div>
</body>
</html>";
            return template.Replace("{{location}}", location).Replace("{{time}}", time);
        }
        public static string PaymentFailedTemplate()
        {
            string template = @"
<!DOCTYPE html>
<html lang='ar'>
<head>
    <meta charset='UTF-8'>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background-color: #f2f4f6; direction: rtl; }
        .card { max-width: 600px; margin: 40px auto; background: #fff; border-radius: 12px; padding: 30px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); }
        .header { font-size: 24px; color: #dc3545; margin-bottom: 10px; display: flex; align-items: center; }
        .icon { font-size: 28px; margin-left: 10px; }
        .message { font-size: 16px; color: #555; line-height: 1.6; }
    </style>
</head>
<body>
    <div class='card'>
        <div class='header'><span class='icon'>❌</span>فشل عملية الدفع</div>
        <div class='message'>
            نأسف، لم تنجح عملية الدفع الأخيرة الخاصة بك. يرجى التحقق من معلومات البطاقة أو المحاولة لاحقًا.
        </div>
    </div>
</body>
</html>";
            return template;
        }

    }
}