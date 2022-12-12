// var ajax = new XMLHttpRequest();
// var method = "GET";
// var url = "Php/data.php";
// var asynchronous = true;

// ajax.open(method, url, asynchronous);
//
// ajax.send();
//
// ajax.onreadystatechange = function ()
// {
//     if(this.readyState == 4 && this.status == 200)
//     {
//         var data = JSON.parse(this.responseText);
//         console.log(data);
//
//         // var htmlcode = data;
//         // const form = document.querySelector(".testing")
//         // form.innerHTML = htmlcode;
//     }
// }

// const form = document.querySelector(".testing")
//
// var htmlcode = '  <form id="form">\n' +
//     '    <div class="form-control-text">\n' +
//     '      <h1>Заголовок</h1>\n' +
//     '    </div>\n' +
//     '    <div class="form-control">\n' +
//     '      <label>Параграф\n' +
//     '      </label>\n' +
//     '    </div>\n' +
//     '    <div class="form-control">\n' +
//     '      <input class ="none" value="\'id_question\', \'id_answer\', " name="3_textinput[]">\n' +
//     '      <label>вопрос</label>\n' +
//     '      <input class = "fix" maxlength="20" name="3_textinput[]" type="text" >\n' +
//     '    </div>\n' +
//     '    <div class="form-control">\n' +
//     '      <input class = "none" value="check-box" name="4_checkbox[]">\n' +
//     '        <label>Вопрос</label>\n' +
//     '      <label><input value="\'id_question\', \'id_answer\'," name="4_checkbox[]" type="checkbox">\n' +
//     '        Вариант 1</label>\n' +
//     '      <label><input value="\'id_question\', \'id_answer\'," name="4_checkbox[]" type="checkbox">\n' +
//     '        Вариант 2</label>\n' +
//     '      <label><input value="\'id_question\', \'id_answer\'," name="4_checkbox[]" type="checkbox">\n' +
//     '        Вариант 3</label>\n' +
//     '    </div>\n' +
//     '    <div class="form-control">\n' +
//     '      <input class = "none" value="check-box-dop" name="5_checkbox[]">\n' +
//     '        <label>Вопрос</label>\n' +
//     '        <label><input value="\'id_question\', \'id_answer\'," name="5_checkbox[]" type="checkbox">\n' +
//     '          Вариант 1</label>\n' +
//     '        <label><input value="\'id_question\', \'id_answer\'," name="5_checkbox[]" type="checkbox">\n' +
//     '          Вариант 2</label>\n' +
//     '        <label><input value="\'id_question\', \'id_answer\'," name="5_checkbox[]" type="checkbox">\n' +
//     '          Вариант 3</label>\n' +
//     '        <div class = "form-other"><input value="\'id_question\', \'id_answer\'," name="5_checkbox[]" type="checkbox">\n' +
//     '          <input name="5_checkbox[]" maxlength="20" type="text" placeholder="Ваш вариант">\n' +
//     '        </div>\n' +
//     '    </div>\n' +
//     '  <div class="form-control">\n' +
//     '    <label>Вопрос</label>\n' +
//     '    <label><input value="\'id_question\', \'id_answer\'," name="6_radio" type="radio">\n' +
//     '      Вариант 1</label>\n' +
//     '    <label><input value="\'id_question\', \'id_answer\'," name="6_radio" type="radio">\n' +
//     '      Вариант 2</label>\n' +
//     '    <label><input value="\'id_question\', \'id_answer\'," name="6_radio" type="radio">\n' +
//     '      Вариант 3</label>\n' +
//     '  </div>\n' +
//     '  <div class="form-control">\n' +
//     '    <input class = "none" value="radio-dop" name="7_radio[]">\n' +
//     '    <label>Вопрос</label>\n' +
//     '    <label><input value="\'id_question\', \'id_answer\'," name="7_radio[]" type="radio">\n' +
//     '      Вариант 1</label>\n' +
//     '    <label><input value="\'id_question\', \'id_answer\'," name="7_radio[]" type="radio">\n' +
//     '      Вариант 2</label>\n' +
//     '    <label><input value="\'id_question\', \'id_answer\'," name="7_radio[]" type="radio">\n' +
//     '      Вариант 3</label>\n' +
//     '    <div class = "form-other"><input value="\'id_question\', \'id_answer\'," name="7_radio[]" type="radio">\n' +
//     '      <input name="7_radio[]" maxlength="20" type="text" placeholder="Ваш вариант">\n' +
//     '    </div>\n' +
//     '  </div>\n' +
//     '\n' +
//     '    <button type="submit" value="submit" onclick="EnterName()">\n' +
//     '      Отправить\n' +
//     '    </button>\n' +
//     '    </form>'
//
// form.innerHTML = htmlcode;