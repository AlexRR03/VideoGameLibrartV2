using ProyectoJuegos.Enums;
namespace ProyectoJuegos.Helpers
{
    public static class HelperListStatus
    {
         public static List<string> GetGameStatusList()
        {
            return Enum.GetNames(typeof(GameStatus)).ToList();
        }
    }
}
