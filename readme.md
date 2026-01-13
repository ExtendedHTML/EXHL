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

-   #### Loop ([specification](./specifications/loop.md))

    **Definition**:
    Loops are used to iterate over **collections, strings, ranges, variables, or function results**, producing repeated HTML elements. The loop is **evaluated once** and generates static HTML output.

    A Loop is defined using the `<loop>` tag.
    The `<loop>` tag must always have a body; it cannot be self-closing.
    The loop body is where all iterations are executed.

    The loop use attributes to get context values and to define operation conditions.

    -   **in:**
        The source to iterate over. Can be: a literal string (iterates over characters), a variable (array or string), or the result of an iterative function. Functions can be called with parameters if applicable.
    -   **as:**
        Defines the name of the loop variable representing the current iteration context. Optional; if omitted, the value is anonymous.
    -   **min:**
        Minimum number of iterations required. If not met, the loop result won't be rendered.
    -   **max:**
        Maximum number of iterations allowed. When reached, the loop stops.
    -   **index:**
        The current iteration index, starting at zero.
    -   **even:**
        Indicates if the current index is even; can be used in styling conditions.
    -   **odd:**
        Indicates if the current index is odd; can be used in styling conditions.

    ```html
    <loop in="@range(0,100)" as="@value">...</loop>
    ```

    ```html
    <loop in="@foobar" as="@item" min="1">...</loop>
    ```

    ```html
    <loop in="@foobar()" as="@item" max="@foobar">...</loop>
    ```

    ```html
    <loop in="hello world" as="@char" min="1" max="100" index="@i" even="@even" odd="@odd">...</loop>
    ```

    **Usage**:
    The `<loop>` tag is used to iterate over a source and repeat HTML structures. Examples of usage include iterating over numeric ranges, strings, variables, and function results.

    **Note:** When "in" attribute is empty or contain null value, the loop will be ignored.

    ```html
    <loop in="@range(0,100)" as="@value" min="1" max="100">
        <h1>Number @value (Index: @index)</h1>
    </loop>
    ```

    ```html
    <loop in="hello world" as="@char">
        <h1>Character: @char</h1>
    </loop>
    ```

    ```html
    <loop in="@items" as="@item">
        <h1>@item.name</h1>
    </loop>
    ```
