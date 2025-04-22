
//using Api.Notifications;
//using ASG.ApiService.Repositories;
//using Microsoft.AspNetCore.SignalR;

//namespace Api.Services
//{
//    public class ServerTimeNotifier : BackgroundService
//    {
//        private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);
//        private readonly ILogger<ServerTimeNotifier> _logger;
//        private readonly IHubContext<NotificationsHub, INotifacationClient> _context;
//        private readonly IUserClaims _userClaims;

//        public ServerTimeNotifier(ILogger<ServerTimeNotifier> logger, IHubContext<NotificationsHub, INotifacationClient> context, IUserClaims userClaims)
//        {
//            _logger = logger;
//            _context = context;
//            _userClaims = userClaims;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            using var timer = new PeriodicTimer(Period);
//            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
//            {
//                var dateTime = DateTime.Now;
//                _logger.LogInformation("Executing {service} {time}", nameof(ServerTimeNotifier), dateTime);

//                // for all users
//                //await _context.Clients.All.ReceiveNotifacation($"Server time = {dateTime}");

//                // for one 
//                await _context.Clients.User(_userClaims.UserId).ReceiveNotifacation($"Server time = {dateTime}");

//            }

//        }
//    }
//}
