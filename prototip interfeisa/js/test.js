

document.addEventListener('click', function(e) {
    if(e.target.tagName === 'a')
    {
        alert(this.dataset.tag)
        e.preventDefault();

    }
});

