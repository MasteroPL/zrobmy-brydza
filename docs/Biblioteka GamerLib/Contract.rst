##############
Contract
##############


##############
Card
##############


.. class::
    Card

Obiekt klasy Card reprezentuje kartę.

+------------------+----------------------------------------------+-------------------------------------------------------------+
| Pole             | Typ                                          | Opis                                                        |
+==================+==============================================+=============================================================+
| ContractHeight   | ContractHeight                               | wyskość kontraktu                                           |
|                  |                                              |                                                             |
|                  |                                              |                                                             |
+------------------+----------------------------------------------+-------------------------------------------------------------+
| ContractColor    | ContractColor                                | kolor kontraktu                                             |
|                  |                                              |                                                             |
|                  |                                              |                                                             |
+------------------+----------------------------------------------+-------------------------------------------------------------+
| XEnabled         | bool                                         | czy kontrakt został skontrowany                             |
|                  |                                              |                                                             |
|                  |                                              |                                                             |
+------------------+----------------------------------------------+-------------------------------------------------------------+
| XXEnabled        | bool                                         | czy kontrakt został zrekontrowany                           |
|                  |                                              |                                                             |
|                  |                                              |                                                             |
+------------------+----------------------------------------------+-------------------------------------------------------------+

.. code-block:: C#

    public ContractHeight ContractHeight { get; set; }
    public ContractColor ContractColor { get; set; }
    public bool XEnabled { get; set; }
    public bool XXEnabled { get; set; }


Konstruktor
============

.. sphinxsharp:method:: public Contract(ContractHeight ContractHeight, ContractColor ContractColor, PlayerTag DeclaredBy, bool XEnabled = false, bool XXEnabled = false)
    :param(1): Wyskość kontraktu.
    :param(2): Kolor kontraktu.
    :param(3): Gracz deklarujący dany kontrakt.
    :param(4): Czy kontrakt został skontrowany.
    :param(5): Czy kontrakt został zrekontrowany.


    .. code-block:: C#

        public Contract(ContractHeight ContractHeight, ContractColor ContractColor, PlayerTag DeclaredBy, bool XEnabled = false, bool XXEnabled = false)
        {
            this.ContractHeight = ContractHeight;
            this.ContractColor = ContractColor;
            this.DeclaredBy = DeclaredBy;
            this.XEnabled = XEnabled;
            this.XXEnabled = XXEnabled;
        }

Metody
======


.. sphinxsharp:method:: public bool IsHigher(Contract Contract)
    :param(1): Kontrakt do porówniania.
    :returns: True, jeżeli kontrakt dla którego ta metoda jest wywoływana jest wyższy, od tego, który został podany jako argument.


Typy wyliczeniowe
=================

Typ wyliczeniowy ContractColor definiuje koloy kontraktów, NONE oznacza pas.

    .. code-block:: C#

        public enum ContractColor
        {
            NONE = -1,
            C = 0,
            D = 1,
            H = 2,
            S = 3,
            NT = 4
        }


Typ wyliczeniowy ContractHeight definiuje wyskości kontraktów, NONE oznacza pas.

    .. code-block:: C#

    public enum ContractHeight
    {
        NONE = -1,
        ONE = 1,
        TWO = 2,
        THREE = 3,
        FOUR = 4,
        FIVE = 5,
        SIX = 6,
        SEVEN = 7
    }