using System.Collections.Generic;

namespace Cooperate_mvc.Models
{
    public class CreateTaskModel
    {
        public TaskModel Task { get; set; }

        public List<GroupModel> Groups { get; set; }

        public List<UserModel> Users { get; set; }
    }
}