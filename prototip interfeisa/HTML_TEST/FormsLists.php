<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content=
    "width=device-width, initial-scale=1.0">
    <link href="../Styles_TEST/Anketa.css" rel="stylesheet" type="text/css">

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
        <nav class="wrapper">
            <nav class="spravkaref__nav"
            <?php
                $host='26.137.232.44';
                $db = 'EdinoeOkno';
                $username = 'Stas';
                $password = '1';
                $dbconn = pg_connect("host=$host port=5432 dbname=$db user=$username password=$password");
                $query = "select id_form ,name_form from forms.forms;";
                $result = pg_query($dbconn,$query );
                $data = array();
                $i = 0;
                $k = true;
                $n = pg_num_rows($result);
                while($i < $n)
                {
                    $data = pg_fetch_row($result,$i);
                    echo '<br>';
                    echo "<a class='centre__nav__link' href=\"Test.php?varname=$data[0]\">$data[1]</a>";
                    $i++;
                }

//                $data = pg_fetch_result($result,1,"name_form");
//
//                print_r($data);
                pg_close($dbconn);
                ?>
        </nav>
        </div>
    </section>
</main>