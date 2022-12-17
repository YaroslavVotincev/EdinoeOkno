<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content=
    "width=device-width, initial-scale=1.0">
    <link href="../Styles_TEST/AnketaList.css" rel="stylesheet" type="text/css">

</head>
<h1>Многофункциональный центр ИжГТУ</h1>
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
    <section class = "intro">
        <div class="wrapper">
            <h1 class="intro__title">
                Доступные анкеты
            </h1>
        </div>
    </section>

        <section class ="benefits">
          <div class = "benefits__wrap">
            <nav class="spravkaref__nav"
            <?php
                $host='26.137.232.44';
                $db = 'EdinoeOkno';
                $username = 'Stas';
                $password = '1';
                $dbconn = pg_connect("host=$host port=5432 dbname=$db user=$username password=$password");
                $query = "select id_form ,name_form,description from forms.forms;";
                $result = pg_query($dbconn,$query );
                $data = array();
                $i = 1;
                $k = true;
                $n = pg_num_rows($result);
                while($i <= $n)
                {
                    $data = pg_fetch_row($result,$i-1);
                    echo "<div class = 'pixel'>";
                    echo "<div class = 'designblock'>
                            <a class='centre__nav__link' href=\"Test.php?varname=$data[0]\">$data[1]</a>
                            <p>$data[2]</p>
                          </div>";
                    $i++;
                }
                pg_close($dbconn);
                ?>
          </div>
        </section>

</main>