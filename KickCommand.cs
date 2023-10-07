using AimRobot.Api;
using AimRobot.Api.command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ar_example_plugin {
    public class KickCommand : ICommandListener {

        public string GetCommandKeyword() {
            return "kick";
        }

        public void OnCommand(CommandData commandData) {
            Robot.GetInstance().GetLogger().Error(commandData.GetSender());
            Robot.GetInstance().GetLogger().Error(Robot.GetInstance().GetGameContext().GetCurrentPlayerName());

            Robot.GetInstance().GetLogger().Error($"{string.Equals(commandData.GetSender(), Robot.GetInstance().GetGameContext().GetCurrentPlayerName())}");
            if (string.Equals(commandData.GetSender(), Robot.GetInstance().GetGameContext().GetCurrentPlayerName())) {
                string playerName = commandData.GetValue<string>("name");
                Robot.GetInstance().GetLogger().Error(playerName);
                Robot.GetInstance().BanPlayer(playerName, $"管理员在游戏中将 {playerName} 屏蔽");
            } else {
                Robot.GetInstance().SendChat("无权执行！");
            }
        }

    }
}
