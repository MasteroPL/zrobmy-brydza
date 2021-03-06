from docutils import nodes

class blogpost_node(nodes.Structural, nodes.Element):
    pass

def visit_blogpost_node(self, node):
    self.body.append(self.starttag(node, 'div'))
    self.body.append()
 
def depart_blogpost_node(self, node):
    self.body.append('</div>')

from docutils.parsers.rst import Directive, directives

class BlogPostDirective(Directive):
    # defines the parameter the directive expects
    # directives.unchanged means you get the raw value from RST
    required_arguments = 0
    optional_arguments = 0
    final_argument_whitespace = True
    option_spec = {
        "date": directives.unchanged,
        "title": directives.unchanged,
        "keywords": directives.unchanged,
        "categories": directives.unchanged
    }

    has_content = True
    add_index = True

    def run(self):
        sett = self.state.document.settings
        language_code = sett.language_code
        env = self.state.document.settings.env

        # gives you access to the parameter stored
        # in the main configuration file (conf.py)
        config = env.config

        # gives you access to the options of the directive
        options = self.options

        # we create a section
        idb = nodes.make_id("blog-" + options["date"] + "-" + options["title"])
        section = nodes.section(ids=[idb])

        # we create a title and we add it to section
        section += nodes.title(options["title"], options["title"])

        # we create the content of the blog post
        # because it contains any kind of RST
        # we parse parse it with function nested_parse
        par = nodes.paragraph()
        self.state.nested_parse(self.content, self.content_offset, par)

        node = blogpost_node()
        node += section
        node += par

        return [ node, ]


def setup(app):
    app.add_node(blogpost_node, html=(visit_blogpost_node, depart_blogpost_node))
    app.add_directive('blogpost', BlogPostDirective)
    

    return {
        "version": "0.1",
    }