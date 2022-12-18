<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content=
    "width=device-width, initial-scale=1.0">
    <link href="../Styles_TEST/Anketa.css" rel="stylesheet" type="text/css">


</head>
<header class="header">
    <div class="wrapper">
        <div class = "header__name">
            <h1>Многофункциональный центр ИжГТУ</h1>
        </div>
        <div class = "header__wrapper">
            <div class = "header__logo">
                <a href ="Main.html" class = "header__logo-link">
                    <img src = "../Styles/Logo/istu-logo.png" alt = "Main" class = "header__logo-pic"/>
                </a>
            </div>

            <nav class ="header__nav">
                <ul class="header__list">
                    <li class ="header__item">
                        <a href ="Main.html" class ="header__link">Главная</a>
                    </li>
                    <li class ="header__item">
                        <a href ="SpravkaRef.html" class ="header__link">Справки и заявления</a>
                    </li>
                    <li class ="header__item">
                        <a href ="FormsLists.php" class ="header__link">Анкетирование</a>
                    </li>
                    <li class ="header__item">
                        <a href ="Questions.html" class ="header__link">Задать вопрос</a>
                    </li>
                </ul>
            </nav>
        </div>
    </div>
</header>

<body>
<main class = "main">
    <section class ="benefits">
        <div class = "benefits__wrap">
            <form action="Php/temp.php" method="POST">
                <?php
                $var_value = $_GET['varname'];
                $host='26.137.232.44';
                $db = 'EdinoeOkno';
                $username = 'Stas';
                $password = '1';
                $dbconn = pg_connect("host=$host port=5432 dbname=$db user=$username password=$password");
                $query = "select html_code from forms.forms where id_form = $var_value;";
                $result = pg_query($dbconn,$query );
                $result = pg_fetch_result($result,0);
                echo $result;
                pg_close($dbconn);
                ?>

                    <button type="submit" value="submit">
                        Отправить
                    </button>
            </form>
        </div>
    </section>

</main>
</body>

</html>

