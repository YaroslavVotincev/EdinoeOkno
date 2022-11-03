<?php
$httpReferer = $_SERVER['HTTP_REFERER'] ?? null;
$xd = explode("/", $httpReferer);
echo $xd[array_key_last($xd)];
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <link href="../Styles/Style.css" rel="stylesheet" type="text/css">
    <meta charset="UTF-8">
    <title>Student</title>
</head>
<body>
<header>
    <br><br><span class="logo"> Многофункциональное окно ИжГТУ </span>
    <nav>
        <a class="Main" href = "Main.html">Главная</a>
    </nav>
</header>
<main>
    <h1>Заявка на получение справок и выписок</h1>
    <form action="../temp%20page/page.php" method="POST">

        <p>Имя<span class="kek">*</span><label>
                <input required type = "text" name = "name">
            </label></p>

        <p>Фамилия<span class="kek">*</span><label>
                <input required type = "text" name = "surname">
            </label></p>

        <p>Отчество<span class="kek">*</span><label>
                <input required type = "text" name = "patronimic">
            </label></p>
        <!-- сделать выбор -->
        <p>Факультет<span class="kek">*</span><label>
                <input required type = "text" name = "fac">
            </label></p>

        <p>Группа<span class="kek">*</span><label>
                <input required type = "text" name = "group">
            </label></p>
        <p>Email<span class="kek">*</span><label>
                <input required type = "text" name = "email">
            </label></p>

        <input type="submit" value="Отправить">
    </form>
</main>
</body>
</html>
