## EXHL - Extended HTML

EXHL is a **specification for extended HTML**.  
It builds on standard HTML and adds features like **variables, conditions, loops, and declarative components** to make HTML more expressive and structured.

> EXHL is a specification intended for template usage.  
> It is not a programming language, library, or framework. Each platform or engine can implement it in its own way.

### Specifications

> Below is a simplified overview of the specifications.

-   #### Component ([specification](./specifications/component.md))

    **Definition**:
    A component is defined using the `component` attribute, whose value serves as a **unique name** within the engine's global system.

    Functions and variables are **inherited from the rendering context**, but they can be **overridden by passing parameters** to the component.
    Components must always be placed at the **root level** of the file. Multiple components per file are allowed, though it is **recommended to define one component per file**.

    **Note:** Functions and variables is detected automatically by references.

    ```html
    <header component="foobar">...</header>
    ```

    **Usage**:
    A component can be used with the `<component />` tag and the `name` attribute, which identifies the component globally in the system.

    Components **do not have a body or child elements (no slots)**.
    Parameters for functions and variables **may be passed**, but are optional.

    **Note:**
    Functions are always references and do not accept parameters. Variables can be literals, references, or the result of a function.

    ```html
    <component name="foobar" />
    ```

    ```html
    <component name="foobar" @foo()="@foobar()" />
    ```

    ```html
    <component name="foobar" @a="foo bar" @b="@variable" @c="function()" />
    ```
