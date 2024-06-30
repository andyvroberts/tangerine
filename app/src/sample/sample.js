// default list
const todos = [
    { task: 'add £100 to bank', done: false },
    { task: 'purchase annual tax certificate', done: false },
    { task: 'live a little', done: false }
]

// HTML construction
const addTodoInput = document.getElementById('td-input')
const addTodoButton = document.getElementById('add-td-btn')
const todoList = document.getElementById('td-list')


// Initialise the view
for (const td of todos) {
    todoList.append(renderTodoInReadMode(td))
}

addTodoInput.addEventListener('input', () => {
    addTodoButton.disabled = addTodoInput.value.length < 3
})

addTodoInput.addEventListener('keydown', ({ key }) => {
    if (key == 'Enter' && addTodoInput.value.length > 3) {
        addTodo()
    }
})

addTodoButton.addEventListener('click', () => {
    addTodo()
})


// Functions
function renderTodoInReadMode(td) {
    const li = document.createElement('li')

    const span = document.createElement('span')
    span.textContent = td.task

    // add a 'done' class to the span for the css to strikethrough.
    if (td.done) {
        span.classList.add('done')
    }

    if (!td.done) {
        span.addEventListener('dblclick', () => {
            const idx = todos.indexOf(td)

            todoList.replaceChild(
                renderTodoInEditMode(td),
                todoList.childNodes[idx]
            )
        })
    }
    li.append(span)

    if (!td.done) {
        const doneBtn = document.createElement('button')
        doneBtn.textContent = 'Done'
        doneBtn.addEventListener('click', () => {
            const idx = todos.indexOf(td)
            removeTodo(idx)
        })
        li.append(doneBtn)
    }

    return li
}

function renderTodoInEditMode(td) {
    const li = document.createElement('li')

    const input = document.createElement('input')
    input.type = 'text'
    input.value = td.task
    li.append(input)

    const saveBtn = document.createElement('button')
    saveBtn.textContent = 'Save'
    saveBtn.addEventListener('click', () => {
        const idx = todos.indexOf(td)
        updateTodo(idx, input.value)
    })
    li.append(saveBtn)

    const cancelBtn = document.createElement('button')
    cancelBtn.textContent = 'Cancel'
    cancelBtn.addEventListener('click', () => {
        const idx = todos.indexOf(td)
        todoList.replaceChild(
            renderTodoInReadMode(td),
            todoList.childNodes[idx]
        )
    })
    li.append(cancelBtn)

    return li
}

function addTodo() {
    const newTd = { task: addTodoInput.value, done: false }

    todos.push(newTd)
    const td = renderTodoInReadMode(newTd)
    todoList.append(td)

    addTodoInput.value = ''
    addTodoButton.disabled = true
}

function removeTodo(idx) {
    //todos.splice(idx, 1)
    //todoList.childNodes[idx].remove()
    todos[idx].done = true
}

function updateTodo(idx, description) {
    todos[idx].task = description
    const td = renderTodoInReadMode(todos[idx])
    todoList.replaceChild(td, todoList.childNodes[idx])
}
