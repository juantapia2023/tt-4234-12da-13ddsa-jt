Prueba Técnica - .NET

Este repositorio contiene la solución a los problemas técnicos planteados.
La solución se ha desarrollado siguiendo principios de arquitectura limpia y SOLID.

Tecnologías y Herramientas
Lenguaje: C# - .NET 9.0
Framework de Pruebas: xUnit
Contenedor de DI: Microsoft.Extensions.DependencyInjection

Informe de Uso de Inteligencia Artificial
A continuación se detalla la interacción con herramientas de IA durante el desarrollo:

1. ¿Utilizaste IA?
Sí. Se utilizó IA como compañero de pensamiento para la validación de arquitecturas y optimización de algoritmos.

2. Modelos Utilizados: Gemini 3 Flash

3. Herramientas
Interfaz Web de Gemini: Para sesiones de lluvia de ideas técnicas y explicaciones teóricas.
Visual Studio: Entornos de desarrollo donde se implementó la lógica.

4. ¿Para qué utilizaste la IA?
	1. Consultoría de Equivalencias Técnicas, dado que tengo un poco mas de tiempo trabajando en Python, 
       utilicé la IA para encontrar las equivalencias más eficientes en el ecosistema .NET 9, como por ejemplo el uso de 
       TimeSpan vs DateTime en .NET, ya que habitualmente en python usaria datetime.timedelta para el manejo y mas eficiente 
       de horas del mismo dia. 
    
5. Prompts y Aclaraciones Relevantes
A continuación, resumo algunos de los prompts utilizados:

    1. Vengo del mundo Python donde usaría una lista de tuplas para intervalos, En .NET, ¿qué tipo de dato es más eficiente 
       para representar horas del mismo día, asi como en python es eficiente usar datetime.timedelta para esos casos.

    2. Técnicamente por qué se prefiere TimeSpan en lugar de DateTime para representar
       diferencias de tiempo del mismo día.
    
    3. En Python suelo usar collections.OrderedDict para implementar un LRU simple. 
       ¿Cuál es el equivalente de collections.OrderedDict en .NET que me permita mantener un buen rendimiento?.


Nota: El uso de la IA se enfocó en cerrar la brecha sintáctica y conceptual para algunos casos entre Python y .NET


Cómo ejecutar el proyecto:
1. Clone el repositorio.
2. Asegúrese de tener instalado el SDK de .NET 9.
3. Para ejecutar las pruebas unitarias: dotnet test
4. Para ejecutar el ejemplo del Problema A: dotnet run --project SalasDeReunion.App 
   O si lo prefiere: dar clic derecho sobre el proyecto SalasDeReunion.App y seleccione la opcion "Establecer como proyecto de inicio",
   luego en el menu supeior de Visual Studio dar clic sobre el icono de play de color verde 
   que esta a la izquierda de nombre "SalasDeReunion.App"  

5. Para ejecutar el ejemplo del Problema B: dotnet run --project CacheConcurrente.App 
   O si lo prefiere: dar clic derecho sobre el proyecto CacheConcurrente.App y seleccione la opcion "Establecer como proyecto de inicio",
   luego en el menu supeior de Visual Studio dar clic sobre el icono de play de color verde 
   que esta a la izquierda de nombre "CacheConcurrente.App"

6. Para ejecutar las pruebas unitarias ejecute el siguiente comando en consola: dotnet test
   o si lo prefiere ejecute cada test desde el explorador (en la opcion "Prueba" del menu de de opciones en la barra superior de Visual Studio
   y seleccione la opcion "Explorador de pruebas" ahi encontrara todos los tests y podra ejecutarlos uno a uno)
