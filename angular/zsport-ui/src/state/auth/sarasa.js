class Persona {
    name = '';
    age = 0;
    email = '';

    constructor(name, age, email) {
        this.name = name;
        this.age = age;
        this.email = email;
    }

    get name() {
        return this.name;
    }

    get age() {
        return this.age;
    }

    get email() {
        return this.email;
    }

    set name(name) {
        this.name = name;
    }

    set age(age) {
        this.age = age;
    }

    set email(email) {
        this.email = email;
    }
}