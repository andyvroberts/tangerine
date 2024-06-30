// default list
const todos = ['add £100 to bank', 'purchase annual tax certificate', 'live a little']

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

addTodoInput.addEventListener('keydown', ({key}) => {
    if (key == 'Enter' && addTodoInput.value.length > 3) {
        addTodo()
    }
})


// Functions
function renderTodoInReadMode(td) {
    // TODO implement this.
}

function addTodo() {
    // TODO implement this.
}
