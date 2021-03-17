/// <reference types="cypress" />

context('Tasks', () => {
    it('Task workflow should works correctly', () => {
        cy.intercept('GET', '/api/tasks').as('getTasks')
        cy.intercept('POST', '/api/tasks').as('addTask')
        cy.intercept('DELETE', '/api/tasks/*').as('removeTask')

        cy.visit('/')
        cy.wait('@getTasks');

        let randomTaskNumber = Math.floor(Math.random()*10000);
        let taskName = `New task from cypress! #${randomTaskNumber}`;

        cy.get('[data-cy="new-task-input"]').type(taskName);
        cy.get('[data-cy="add-task-button"]').click();
        cy.wait('@addTask');
        cy.get('[data-cy="new-task-input"]').invoke('val').should('be.empty');

        cy.wait('@getTasks')
        cy.get('[data-cy="task-list"] li').contains(taskName);

        cy.get('[data-cy="task-list"] li').contains(taskName).find('[data-cy="remove-task-button"]').click()
        cy.wait('@removeTask')

        cy.wait('@getTasks')
        cy.contains(taskName).should('not.exist');

        cy.get('[data-cy="new-task-input"]').type(taskName);
        cy.get('[data-cy="add-task-button"]').click();
        cy.wait('@addTask');
        cy.get('[data-cy="new-task-input"]').type(taskName);
        cy.get('[data-cy="add-task-button"]').click();
        cy.wait('@addTask');

        cy.contains("Something went wrong").should('exist')
    })
})