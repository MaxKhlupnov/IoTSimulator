# IoTSimulator
Sample IoT simulation app for Yandex IoT Core
Консольное приложение, котрое эмулирует одно или несколько IoT устройств и отправляет в топик MQTT 
<a href="https://cloud.yandex.ru/docs/iot-core/">сервиса IoT Core</a>  JSON-сообщения.

Приложение написано на .NET Core, работа тестировалась на Windows, Mac и Ubuntu.
<P>Видео с описанием порядка настройки и запуска приложения см. <a href="https://youtu.be/xPsf3muVTTs">здесь</a></P>

Для дальнейшей обработки данных на стороне облака рекомендуется ознакомиться с примером 
<a href="https://github.com/MaxKhlupnov/IoTCoreAdapter">IoTAdapter</a>

Для работы примера нужно:
<ol>
<li>Установить на компьютер <a href='https://dotnet.microsoft.com/download'>.NET Core SDK</a> для Вашей ОС</li>
<li>Отредактировать файл appsettings.json в котором указать параметры подключения устройств-эмуляторов к топикам IoT Core</li>
<li>Скомпилировать приложение командой: <pre>dotnet build</pre></li>
<li>Перейти в папку bin/Debug/netcoreapp3.1 и запустить приложение командой: <pre>dotnet IoTSimulation.dll</pre></li>
<li>Усли Вы используете сертификаты устройств - они должны так же находиться папке /Data </li>
</ol>
